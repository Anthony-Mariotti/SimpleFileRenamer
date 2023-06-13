namespace SimpleFileRenamer.Models.Sessions;
public class LiveSession
{
    public Dictionary<string, int> SessionNumbers { get; set; } = new();

    public dynamic? Data { get; set; }
}


