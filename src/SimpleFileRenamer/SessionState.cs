namespace SimpleFileRenamer;
public class SessionState
{
    public string PersonName { get; set; }

    public List<string> CreatedFiles { get; set; }

    public int LastSessionNumber { get; set; } = 0;

    public SessionState()
    {
        PersonName = string.Empty;
        CreatedFiles = new List<string>();
    }
}
