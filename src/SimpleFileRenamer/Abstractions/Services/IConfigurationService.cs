using SimpleFileRenamer.Models.Config;

namespace SimpleFileRenamer.Abstractions.Services;

public interface IConfigurationService
{
    public SimpleFileRenamerConfig Value { get; }

    public void ReloadConfiguration();

    public void Save();
}
