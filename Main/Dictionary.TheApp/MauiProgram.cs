using Dictionary.Shared.Extensions;
using System.Globalization;

#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace Dictionary.TheApp;

public static class MauiProgram
{
    private const string DB_FILE_NAME = "de.sqlite";

    public static MauiApp CreateMauiApp()
    {
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("cs-CZ");
        var builder = MauiApp.CreateBuilder();
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_FILE_NAME);

        // Copy the database file to an app-dedicated folder in the current system AppData.
        // This is the accepted way of handling resources in cross-platform MAUI.
        // Do not try to use "Resources/Raw/{DB_FILE_NAME}" directly as the database path. It will work on Windows, but not on Android.
        using (var dbAssetStream = FileSystem.OpenAppPackageFileAsync($"Resources/Raw/{DB_FILE_NAME}").GetAwaiter().GetResult())
        using (var dbFileStream = new FileStream(dbPath, FileMode.OpenOrCreate))
        {
            dbAssetStream.CopyTo(dbFileStream);
        }

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services
            .ConfigureSharedInternalDependencies(dbPath)
            .AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
