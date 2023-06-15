namespace SimpleFileRenamer.Models.Session;

public class LiveModeFile
{
    public string Hash { get; set; } = default!;

    public string Path { get; set; } = default!;

    public List<LiveModeRow> Rows { get; set; } = new();
}
