using Serilog;
using SimpleFileRenamer.Abstractions.Factory;
using SimpleFileRenamer.Abstractions.Services;

namespace SimpleFileRenamer;
public partial class LiveModeWindow : Form
{
    private readonly IWindowFactory _windowFactory;
    private readonly IDataImportService _importService;
    private readonly ILiveModeCacheService _liveModeCache;

    public LiveModeWindow(IWindowFactory windowFactory, IDataImportService importService, ILiveModeCacheService liveModeCache)
    {
        Log.Verbose("Initializing Live Mode Window");
        _windowFactory = windowFactory;
        _importService = importService;
        _liveModeCache = liveModeCache;

        InitializeComponent();

        PeopleListView.View = View.Details;
    }

    private void ImportButton_Click(object sender, EventArgs e)
    {
        Log.Information("Import button clicked. Displaying file dialog to select file for import");

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

            try
            {
                var hasHeader = MessageBox.Show(
                    "Does the file contain a header row?",
                    "Header Row", MessageBoxButtons.YesNo) == DialogResult.Yes;

                Log.Information("User selected that file has header row: {HasHeader}", hasHeader);

                // Depending on the file type, read the file
                if (Path.GetExtension(filePath).ToLower().Contains("csv"))
                {
                    Log.Information("Importing data from CSV file {FilePath}", filePath);

                    // Process CSV
                    var people = _importService.FromCSV(filePath, out var headers, hasHeader);

                    Log.Information("Successfully imported {RecordCount} records from CSV file {FilePath}", people.Count, filePath);

                    _liveModeCache.SelectFile(filePath);

                    // Populate the list in your application
                    PopulatePeopleList(people, headers);
                }
                else
                {
                    Log.Information("Importing data from Excel file {FilePath}", filePath);

                    // Process EXCEL
                    var people = _importService.FromExcel(filePath, out var headers, hasHeader);

                    Log.Information("Successfully imported {RecordCount} records from Excel file {FilePath}", people.Count, filePath);

                    _liveModeCache.SelectFile(filePath);

                    // Populate the list in your application
                    PopulatePeopleList(people, headers);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while importing data");
                MessageBox.Show("Error importing data: " + ex.Message, "Failed importing data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void PopulatePeopleList(List<List<string>> data, List<string> headers)
    {
        Log.Information("Attempting to populate data list with {DataNumber} records and {HeaderNumber} headers", data.Count, headers.Count);
        try
        {
            PeopleListView.Items.Clear();
            PeopleListView.Columns.Clear();

            // Add fixed 'Status' column
            PeopleListView.Columns.Add("Status");

            // Set column headers
            if (headers.Count > 0)
            {
                foreach (string header in headers)
                {
                    PeopleListView.Columns.Add(header);
                }
            }
            else
            {
                Log.Debug("No headers were supplied. Generating default column numbers");
                var maxColumns = data.Any()
                    ? data.Max(row => row.Count)
                    : 0;

                for (int i = 1; i <= maxColumns; i++)
                {
                    PeopleListView.Columns.Add($"Column{i}");
                }
                Log.Verbose("Generated {MaxColumns} default columns", maxColumns);
            }

            var rowIndex = 0;
            foreach (var rowData in data)
            {
                var cachedRow = _liveModeCache.GetOrCreateCachedRow(rowIndex, rowData[0]);

                // The first column (Status) should be empty initially
                var item = new ListViewItem(cachedRow.Status)
                {
                    Tag = cachedRow.Hash
                };

                foreach (string columnData in rowData)
                {
                    item.SubItems.Add(columnData); // This adds data starting from the second column
                }

                PeopleListView.Items.Add(item);

                rowIndex++;
            }

            // Auto resize columns
            foreach (ColumnHeader column in PeopleListView.Columns)
            {
                column.Width = -2;
            }
            Log.Information("Successfully populated data list with {RecordCount} records and created {ColumnCount} columns",
                data.Count, PeopleListView.Columns.Count - 1);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to populate data list with {RecordCount} records and created {ColumnCount} columns",
                data.Count, PeopleListView.Columns.Count - 1);
        }
    }

    private void PeopleListView_ItemActivate(object sender, EventArgs e)
    {
        if (PeopleListView.SelectedItems.Count > 0)
        {
            var selectedItem = PeopleListView.SelectedItems[0];
            var rowHash = selectedItem.Tag as string;

            if (string.IsNullOrWhiteSpace(rowHash))
            {
                Log.Error("Selected item has an invalid row hash");
                MessageBox.Show("Failed to load session");
                return;
            }

            if (selectedItem.Text == "Completed" && MessageBox.Show(
                    "This item is completed. Do you want to start a new session?",
                    "Item already completed",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
            {
                Log.Information("Item selected: {RowHash}. Already completed and user canceled", rowHash);
                return;
            }

            Log.Information("Item selected: {RowHash}. Opening session window...", rowHash);

            // Create and open the SessionForm
            using var sessionWindow = _windowFactory.CreateSessionWindow();

            sessionWindow.SetSession(rowHash, selectedItem);

            if (!sessionWindow.IsDisposed)
            {
                _ = sessionWindow.ShowDialog(this);
                Log.Information("Session window opened successfully for {DataItem}", rowHash);
                return;
            }

            // Disposed is expected because we closed it before opening... what else could dispose it...?
            //Log.Error("The session window for {DataItem} did not open as it was already disposed", rowHash);
        }
        else
        {
            Log.Warning("No item selected to open a session window");
        }
    }

    private void SettingsMenuItem_Click(object sender, EventArgs e)
    {
        using var configWindow = _windowFactory.CreateSessionConfigurationWindow();
        _ = configWindow.ShowDialog();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }
}
