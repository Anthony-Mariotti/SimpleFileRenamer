namespace SimpleFileRenamer.Abstractions.Factory;

public interface IWindowFactory
{
    SessionWindow CreateSessionWindow();

    SessionConfigurationWindow CreateSessionConfigurationWindow();

    LiveModeWindow CreateLiveModeWindow();

    FormatWindow CreateFormatWindow();
}
