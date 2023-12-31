﻿namespace SimpleFileRenamer.Models.Config;
public class LiveModeConfig
{
    public string WatchedFolder { get; set; } = default!;

    public string DestinationFolder { get; set; } = default!;

    public Dictionary<string, int> LastSessionNumbers { get; set; } = new Dictionary<string, int>();

    public List<string> MonitoredExtensions { get; set; } = new List<string>();
}
