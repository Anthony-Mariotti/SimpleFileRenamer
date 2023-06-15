using NPOI.XSSF.UserModel;
using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using System.Diagnostics;

namespace SimpleFileRenamer.Services;
public class DataImportService : IDataImportService
{
    public List<List<string>> FromCSV(string csvFilePath, out List<string> headers, bool hasHeader = false)
    {
        Log.Debug("FromCSV started for file {FilePath} with hasHeader={HasHeader}", csvFilePath, hasHeader);
        var stopwatch = Stopwatch.StartNew();

        var data = new List<List<string>>();
        headers = new List<string>();

        using var sr = new StreamReader(csvFilePath);

        var rowIndex = 0;
        string? line;

        while ((line = sr.ReadLine()) != null)
        {
            string[] columns = line.Split(',');

            if (hasHeader && rowIndex == 0)
            {
                Log.Verbose("FromCSV found {HeaderCount} headers", columns.Length);
                headers.AddRange(columns);
            }
            else
            {
                data.Add(new List<string>(columns));
            }
            rowIndex++;
        }

        stopwatch.Stop();
        Log.Debug("FromCSV completed for file {FilePath}. Imported {RecordCount} records in {ElapsedMilliseconds}ms",
            csvFilePath, data.Count, stopwatch.ElapsedMilliseconds);

        return data;
    }

    public List<List<string>> FromExcel(string excelFilePath, out List<string> headers, bool hasHeader = false)
    {
        Log.Debug("FromExcel started for file {FilePath} with hasHeader={HasHeader}", excelFilePath, hasHeader);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var data = new List<List<string>>();
        headers = new List<string>();

        using var file = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);

        var workbook = new XSSFWorkbook(file);
        var sheet = workbook.GetSheetAt(0);

        var startRowIndex = 0;

        if (hasHeader)
        {
            Log.Verbose("FromExcel started reading the header row");
            var headerRow = sheet.GetRow(0);
            foreach (var cell in headerRow)
            {
                headers.Add(cell.StringCellValue);
            }   

            Log.Verbose("FromExcel found {HeaderCount} headers", headers.Count);

            startRowIndex = 1; // Skip the header row
        }

        Log.Verbose("FromExcel started iterating over data rows");
        for (int rowIndex = startRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row != null)
            {
                var rowData = new List<string>();
                for (int colIndex = 0; colIndex < row.LastCellNum; colIndex++)
                {
                    var cell = row.GetCell(colIndex);
                    rowData.Add(cell?.ToString() ?? string.Empty);
                }

                // Too Much Logging?
                //Log.Verbose("FromExcel read data row {RowIndex} with '{RowValues}'", rowIndex, string.Join(", ", rowData));
                data.Add(rowData);
            }
        }

        stopwatch.Stop();
        Log.Debug("FromExcel completed for file {FilePath}. Imported {RecordCount} records in {ElapsedMilliseconds}ms",
            excelFilePath, data.Count, stopwatch.ElapsedMilliseconds);
        return data;
    }
}