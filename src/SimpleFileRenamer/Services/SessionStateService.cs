using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Abstractions.Utilities;
using SimpleFileRenamer.Models.Sessions;

namespace SimpleFileRenamer.Services;

public class SessionStateService : ISessionStateService
{
    private readonly IFileSerializer _serializer;
    private readonly string _sessionFolder;
    private SessionState? _currentSession;

    private string SafeFileName => 
        Current?.Name.Trim().Replace(" ", "_").ToLower()
        ?? string.Empty;

    private string SessionFile => Path.Combine(_sessionFolder, $"session_{SafeFileName}.json");

    public SessionState? Current => _currentSession;

    public SessionStateService(IFileSerializer fileSerializer)
    {
        _serializer = fileSerializer;
        _sessionFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions");

        if (!Directory.Exists(_sessionFolder))
        {
            Directory.CreateDirectory(_sessionFolder);
        }
    }

    public void Create(string name)
    {
        Log.Debug("Creating new session state");
        _currentSession = new SessionState
        {
            Name = name
        };
    }

    public bool Load(string name)
    {
        Log.Verbose("Loading session state");
        try
        {
            Create(name);

            if (!File.Exists(SessionFile))
            {
                Log.Verbose("Session state file does not exists at {FilePath}", SessionFile);
                // No session file found, created new session
                return true;
            }

            var serialized = File.ReadAllText(SessionFile);
            _currentSession = _serializer.Deserialize<SessionState>(serialized);

            if (_currentSession != null)
            {
                Log.Verbose("Session state file exists at {FilePath}", SessionFile);
                // Session file was found and loaded
                return true;
            }

            Create(name);
            Log.Warning("Failed to load existing session file {FileName}", SafeFileName);

            // Session failed to load, createe new session
            return false;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load session file {FileName}", SafeFileName);
            return false;
        }
    }

    public void Save()
    {
        Log.Debug("Saving session state");
        try
        {
            if (_currentSession == null)
            {
                return;
            }

            var serialized = _serializer.Serialize(_currentSession);
            File.WriteAllText(SessionFile, serialized);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to save session file {FileName}", SafeFileName);
            return;
        }
    }

    public void Finish()
    {
        Log.Debug("Finishing session state");
        try
        {
            if (File.Exists(SessionFile))
            {
                Log.Verbose("Found and removing session file {FilePath}", SessionFile);
                File.Delete(SessionFile);
            }

            return;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete session file {FileName}", SafeFileName);
            return;
        }
    }
}
