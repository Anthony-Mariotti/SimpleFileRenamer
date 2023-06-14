using Microsoft.Extensions.DependencyInjection;
using SimpleFileRenamer.Abstractions.Factory;

namespace SimpleFileRenamer.Services.Factory;
public class WindowFactory : IWindowFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WindowFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public RenameConfigurationWindow CreateRenameConfigurationWindow() =>
        _serviceProvider.GetRequiredService<RenameConfigurationWindow>();

    public LiveModeWindow CreateLiveModeWindow() =>
        _serviceProvider.GetRequiredService<LiveModeWindow>();

    public SessionConfigurationWindow CreateSessionConfigurationWindow() =>
        _serviceProvider.GetRequiredService<SessionConfigurationWindow>();

    public SessionWindow CreateSessionWindow() =>
        _serviceProvider.GetRequiredService<SessionWindow>();


}
