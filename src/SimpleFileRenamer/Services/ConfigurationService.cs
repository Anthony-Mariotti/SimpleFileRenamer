using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Abstractions.Utilities;
using SimpleFileRenamer.Models.Config;

namespace SimpleFileRenamer.Services;
public class ConfigurationService : IConfigurationService
{
    private readonly string _configFilePath;
    private readonly IFileSerializer _serializer;
    private SimpleFileRenamerConfig _config = new();

    public SimpleFileRenamerConfig Value => _config;

    public ConfigurationService(IFileSerializer serializer)
    {
        _serializer = serializer;
        _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        LoadConfiguration();
    }

    public void ReloadConfiguration() => LoadConfiguration();

    public void Save()
    {
        try
        {
            var configJson = _serializer.Serialize(_config);
            File.WriteAllText(_configFilePath, configJson);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to save configuration to file");
        }
    }

    private void LoadConfiguration()
    {
        try
        {
            if (File.Exists(_configFilePath))
            {
                var configJson = File.ReadAllText(_configFilePath);
                _config = _serializer.Deserialize<SimpleFileRenamerConfig>(configJson) ?? new();
            }
            else
            {
                _config = new();
                Save();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to read configuraiton from file");
            _config = new();
        }
    }
}
