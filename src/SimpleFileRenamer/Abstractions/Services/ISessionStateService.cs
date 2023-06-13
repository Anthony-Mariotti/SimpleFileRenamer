using SimpleFileRenamer.Models.Sessions;

namespace SimpleFileRenamer.Abstractions.Services;

public interface ISessionStateService
{
    public SessionState? Current { get; }

    public void Create(string name);

    public bool Load(string name);

    public void Save();

    public void Finish();
}