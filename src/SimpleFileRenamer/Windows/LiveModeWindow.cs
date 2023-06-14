using NPOI.XSSF.UserModel;
using Serilog;
using SimpleFileRenamer.Abstractions.Factory;
using System.Diagnostics;

namespace SimpleFileRenamer;
public partial class LiveModeWindow : Form
{
    private readonly IWindowFactory _windowFactory;

    public LiveModeWindow(IWindowFactory windowFactory)
    {
        Log.Verbose("Initializing Live Mode Window");
        _windowFactory = windowFactory;

        InitializeComponent();

        PeopleListView.View = View.Details;
    }

    private void ImportButton_Click(object sender, EventArgs e)
    {
        Log.Verbose("Displaying import file dialog");
        // Create an OpenFileDialog to select the Excel/CSV file
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|CSV Files|*.csv",
            Title = "Select an Excel or CSV File"
        };

        // Show the dialog and get result
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            Log.Debug("Selecting file {File} for import", openFileDialog.FileName);
            var filePath = openFileDialog.FileName;

            // Depending on the file type, read the file
            if (Path.GetExtension(filePath).ToLower().Contains("csv"))
            {
                Log.Debug("Extesion read as CSV file");
                // Process CSV
                var people = ReadDataFromCSV(filePath, out var headers);

                // Populate the list in your application
                PopulatePeopleList(people, headers);
            }
            else
            {
                Log.Debug("File has {Extension} and reading as excel", Path.GetExtension(filePath));
                // Process EXCEL
                var people = ReadDataFromExcel(filePath, out var headers);

                // Populate the list in your application
                PopulatePeopleList(people, headers);
            }
        }
    }

    private List<List<string>> ReadDataFromCSV(string filePath, out List<string> headers)
    {
        Log.Verbose("Attempting to read from CSV file {File}", filePath);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var data = new List<List<string>>();
        headers = new List<string>();

        try
        {
            using var sr = new StreamReader(filePath);

            var rowIndex = 0;
            string? line;

            // Ask the user if the file has a header row
            var dialogResult = rowIndex == 0
                ? MessageBox.Show("Does the file contain a header row?", "Header Row", MessageBoxButtons.YesNo)
                : DialogResult.No;

            Log.Verbose("Response to header row was {Response}", dialogResult == DialogResult.Yes ? "Yes" : "No");

            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');

                if (dialogResult == DialogResult.Yes && rowIndex == 0)
                {
                    headers.AddRange(columns);
                }
                else
                {
                    data.Add(new List<string>(columns));
                }
                rowIndex++;
            }

            stopwatch.Stop();
            Log.Debug("Generated {DataNumber} data lines and {HeaderNumber} headers in {ElapsedMilliseconds}ms", data.Count, headers.Count, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while reading the CSV file");
            MessageBox.Show("Error reading the CSV file: " + ex.Message,
                "Failed reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return data;
    }

    private List<List<string>> ReadDataFromExcel(string filePath, out List<string> headers)
    {
        Log.Verbose("Attempting to read from Excel file {File}", filePath);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var data = new List<List<string>>();
        headers = new List<string>();

        try
        {
            using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var workbook = new XSSFWorkbook(file);
            var sheet = workbook.GetSheetAt(0);

            var startRowIndex = 0;

            // Ask the user if the file has a header row
            var dialogResult = MessageBox.Show("Does the file contain a header row?", "Header Row", MessageBoxButtons.YesNo);
            Log.Verbose("Response to header row was {Response}", dialogResult == DialogResult.Yes ? "Yes" : "No");

            if (dialogResult == DialogResult.Yes)
            {
                var headerRow = sheet.GetRow(0);
                foreach (var cell in headerRow)
                {
                    headers.Add(cell.StringCellValue);
                }
                startRowIndex = 1; // Skip the header row
            }

            // Read data
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
                    data.Add(rowData);
                }
            }

            stopwatch.Stop();
            Log.Debug("Generated {DataNumber} data lines and {HeaderNumber} headers in {ElapsedMilliseconds}ms", data.Count, headers.Count, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while reading the CSV file");
            MessageBox.Show("Error reading the Excel file: " + ex.Message,
                "Failed reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return data;
    }

    private void PopulatePeopleList(List<List<string>> data, List<string> headers)
    {
        Log.Verbose("Attempting to populate data list with {DataNumber} records and {HeaderNumber} headers", data.Count, headers.Count);
        PeopleListView.Items.Clear();
        PeopleListView.Columns.Clear();

        // Add fixed 'Status' column
        PeopleListView.Columns.Add("Status");

        // Set column headers
        foreach (string header in headers)
        {
            PeopleListView.Columns.Add(header);
        }

        // Add items
        foreach (var rowData in data)
        {
            var item = new ListViewItem("Not Started"); // The first column (Status) should be empty initially

            foreach (string columnData in rowData)
            {
                item.SubItems.Add(columnData); // This adds data starting from the second column
            }

            PeopleListView.Items.Add(item);
        }

        // Auto resize columns
        foreach (ColumnHeader column in PeopleListView.Columns)
        {
            column.Width = -2;
        }
    }

    private void PeopleListView_ItemActivate(object sender, EventArgs e)
    {
        if (PeopleListView.SelectedItems.Count > 0)
        {
            var selectedItem = PeopleListView.SelectedItems[0];
            var personName = selectedItem.SubItems[1].Text; // Assuming name is in the second column
            Log.Debug("The item {Data} has been selected for a session", personName);

            // Create and open the SessionForm
            using var sessionWindow = _windowFactory.CreateSessionWindow();

            if (!sessionWindow.IsDisposed)
            {
                _ = sessionWindow.ShowDialog(this, personName, selectedItem);
                return;
            }

            Log.Error("The session window did not start as it was already disposed");
        }
    }

    private void SettingsMenuItem_Click(object sender, EventArgs e)
    {
        using var configWindow = _windowFactory.CreateSessionConfigurationWindow();
        _ = configWindow.ShowDialog();
    }
}
