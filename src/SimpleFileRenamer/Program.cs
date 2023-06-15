using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SimpleFileRenamer.Abstractions.Factory;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Abstractions.Utilities;
using SimpleFileRenamer.Services;
using SimpleFileRenamer.Services.Factory;
using SimpleFileRenamer.Utilities;

namespace SimpleFileRenamer;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Create logging provider
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs\\simplefilerenamer.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Application Starting Up");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Create a service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the DI container
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Resolve the MainWindow from the DI container
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

            Application.Run(mainWindow);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred that has caused the application to exit");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IFileSerializer, JsonFileSerializer>();
        services.AddSingleton<IWindowFactory, WindowFactory>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IRenameHistoryService, RenameHistoryService>();
        services.AddSingleton<IDataImportService, DataImportService>();
        services.AddSingleton<ILiveModeCacheService, LiveModeCacheService>();

        services.AddScoped<IRenameService, RenameService>();

        // Available Windows
        services.AddTransient<MainWindow>();
        services.AddTransient<RenameConfigurationWindow>();
        services.AddTransient<LiveModeWindow>();
        services.AddTransient<SessionWindow>();
        services.AddTransient<SessionConfigurationWindow>();
    }
}