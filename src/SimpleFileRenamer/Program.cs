using Serilog;

namespace SimpleFileRenamer;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
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
            Application.Run(new MainWindow());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start correctly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}