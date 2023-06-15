using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Abstractions.Utilities;
using SimpleFileRenamer.Models.Session;
using System.Security.Cryptography;
using System.Text;

namespace SimpleFileRenamer.Services;

public class LiveModeCacheService : ILiveModeCacheService
{
    private const string CacheFolderName = "cache";
    private const string CacheFileName = "lm.cache.json";

    private readonly string _cacheFolder;
    private readonly string _cachePath;
    private readonly IFileSerializer _serializer;
    private readonly LiveModeCache _cache;

    private LiveModeFile _currentFile = default!;

    public LiveModeCacheService(IFileSerializer serializer)
    {
        _serializer = serializer;

        _cacheFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheFolderName);
        _cachePath = Path.Combine(_cacheFolder, CacheFileName);

        if (File.Exists(_cachePath))
        {
            var configJson = File.ReadAllText(_cachePath);
            _cache = _serializer.Deserialize<LiveModeCache>(configJson) ?? new();
        }
        else
        {
            _cache = new();
            Save();
        }
    }

    public void SelectFile(string path)
    {
        var pathHash = GetPathHash(path);
        var currentFile = _cache.Files.FirstOrDefault(x => x.Hash == pathHash);

        if (currentFile == null)
        {
            _currentFile = new LiveModeFile
            {
                Hash = pathHash,
                Path = path
            };

            _cache.Files.Add(_currentFile);
            return;
        }

        _currentFile = currentFile;
    }

    public LiveModeRow GetOrCreateCachedRow(int rowIndex, string firstColumn)
    {
        var rowHash = GetRowHash(rowIndex, firstColumn);

        var cachedRow = _currentFile.Rows.FirstOrDefault(row => row.Hash == rowHash);
        if (cachedRow == null)
        {
            cachedRow = new LiveModeRow
            {
                Hash = rowHash,
            };
            _currentFile.Rows.Add(cachedRow);
        }

        return cachedRow;
    }

    public LiveSession GetLiveSession(string rowHash)
    {
        var row = _currentFile.Rows.FirstOrDefault(row => row.Hash == rowHash);
        if (row == null)
        {
            row = new LiveModeRow
            {
                Hash = rowHash
            };

            _currentFile.Rows.Add(row);
            return row.Session;
        }

        return row.Session;
    }

    public void UpdateLiveSession(string rowHash, LiveSession session)
    {
        var row = _currentFile.Rows.FirstOrDefault(row => row.Hash == rowHash);
        if (row == null)
        {
            row = new LiveModeRow
            {
                Hash = rowHash,
                Session = session
            };

            _currentFile.Rows.Add(row);
            return;
        }

        row.Session = session;
    }

    public void SetRowStatus(string rowHash, string status)
    {
        var row = _currentFile.Rows.Single(row => row.Hash == rowHash);
        row.Status = status;
    }


    public bool Save()
    {
        if (_cache != null)
        {
            try
            {

                if (!Directory.Exists(_cacheFolder))
                {
                    Directory.CreateDirectory(_cacheFolder);
                }

                var rawCache = _serializer.Serialize(_cache);
                File.WriteAllText(_cachePath, rawCache);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save live mode cache to file");
                return false;
            }
        }

        return false;
    }

    public void DeleteCache()
    {
        _cache.Files.Clear();
        
        if (_currentFile is not null or not default(LiveModeFile))
        {
            _cache.Files.Add(_currentFile);
        }

        Save();
    }

    private string GetPathHash(string path)
    {
        var pathBytes = Encoding.UTF8.GetBytes(path);
        var hashBytes = SHA256.HashData(pathBytes);
        var builder = new StringBuilder();
        foreach ( var item in hashBytes )
        {
            builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }

    private string GetRowHash(int rowIndex, string firstColumn)
    {
        var rawIdentifier = $"{rowIndex}|{firstColumn}";
        var rawBytes = Encoding.UTF8.GetBytes(rawIdentifier);
        var hashBytes = SHA256.HashData(rawBytes);
        var builder = new StringBuilder();
        foreach(var item in hashBytes)
        {
            builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }
}
