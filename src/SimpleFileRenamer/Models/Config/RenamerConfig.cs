namespace SimpleFileRenamer.Models.Config;

public class RenamerConfig
{
    public char Delimiter { get; set; } = '-';

    public string Format { get; set; } = "qr_{0}_{1}";
}