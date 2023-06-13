namespace SimpleFileRenamer.Models.Config;

public class SimpleFileRenamerConfig
{
    public LiveModeConfig LiveMode { get; set; } = new();

    public RenamerConfig Renamer { get; set; } = new();
}
