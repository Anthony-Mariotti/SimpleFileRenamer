using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SimpleFileRenamer;
public partial class LiveMode : Form
{
    public LiveMode()
    {
        InitializeComponent();

        PeopleListView.View = View.Details;
    }

    private void ImportButton_Click(object sender, EventArgs e)
    {
        // Create an OpenFileDialog to select the Excel/CSV file
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|CSV Files|*.csv",
            Title = "Select an Excel or CSV File"
        };

        // Show the dialog and get result
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            var filePath = openFileDialog.FileName;

            // Depending on the file type, read the file
            if (Path.GetExtension(filePath).ToLower().Contains("csv"))
            {
                // Process CSV
                var people = ReadDataFromCSV(filePath, out var headers);

                // Populate the list in your application
                PopulatePeopleList(people, headers);
            }
            else
            {
                // Process EXCEL
                var people = ReadDataFromExcel(filePath, out var headers);

                // Populate the list in your application
                PopulatePeopleList(people, headers);
            }
        }
    }

    private List<List<string>> ReadDataFromCSV(string filePath, out List<string> headers)
    {
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
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error reading the CSV file: " + ex.Message,
                "Failed reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return data;
    }

    private List<List<string>> ReadDataFromExcel(string filePath, out List<string> headers)
    {
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

        }
        catch (Exception ex)
        {
            MessageBox.Show("Error reading the Excel file: " + ex.Message,
                "Failed reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return data;
    }

    private void PopulatePeopleList(List<List<string>> data, List<string> headers)
    {
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
            ListViewItem selectedItem = PeopleListView.SelectedItems[0];
            string personName = selectedItem.SubItems[1].Text; // Assuming name is in the second column

            // Create and open the SessionForm
            using var sessionForm = new SessionWindow(personName, selectedItem);

            if (!sessionForm.IsDisposed)
            {
                _ = sessionForm.ShowDialog(this);
            }
        }
    }

    private void SettingsMenuItem_Click(object sender, EventArgs e)
    {
        using var configWindow = new SessionConfigurationWindow();
        _ = configWindow.ShowDialog();
    }
}
