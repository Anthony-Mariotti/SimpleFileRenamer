using SimpleFileRenamer.Models.Session;

namespace SimpleFileRenamer.Abstractions.Services;

public interface ILiveModeCacheService
{
    void SelectFile(string path);

    LiveModeRow GetOrCreateCachedRow(int rowIndex, string firstColumn);

    LiveSession GetLiveSession(string rowHash);

    void UpdateLiveSession(string rowHash, LiveSession session);

    void SetRowStatus(string rowHash, string status);

    bool Save();

    void DeleteCache();
}
