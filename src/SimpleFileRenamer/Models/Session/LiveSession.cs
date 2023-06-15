namespace SimpleFileRenamer.Models.Session;

public class LiveSession
{
    public int SessionId { get; set; } = 0;

    public List<SessionFile> Files { get; set; } = new();
}
