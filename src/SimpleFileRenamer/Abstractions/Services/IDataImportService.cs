namespace SimpleFileRenamer.Abstractions.Services;
public interface IDataImportService
{
    List<List<string>> FromCSV(string csvFilePath, out List<string> headers, bool hasHeader = false);

    List<List<string>> FromExcel(string excelFilePath, out List<string> headers, bool hasHeader = false);
}
