namespace SimpleFileRenamer.Models.Sessions;
public class SessionState
{
    public string Name { get; set; }

    public List<string> CreatedFiles { get; set; }

    public int LastSessionNumber { get; set; } = 0;

    public SessionState()
    {
        Name = string.Empty;
        CreatedFiles = new List<string>();
    }
}
