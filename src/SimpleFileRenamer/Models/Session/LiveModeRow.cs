namespace SimpleFileRenamer.Models.Session;

public class LiveModeRow
{
    public string Hash { get; set; } = default!;

    public string Status { get; set; } = "Not Started";

    public LiveSession Session { get; set; } = new();
}
