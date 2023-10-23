using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using WorkTimer.Database;
using WorkTimer.View;
using WorkTimer.ViewModel;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace WorkTimer;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        builder.Logging.AddDebug();

        // WorktimeRepository als Singleton initialisieren, damit alle ViewModels darauf zugreifen können
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "worktime.db3");
        builder.Services.AddSingleton<WorktimeRepository>(s => ActivatorUtilities.CreateInstance<WorktimeRepository>(s, dbPath));

        // Alle Pages und Viewmodels als Singleton initialisieren
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();

        builder.Services.AddTransient<OverviewPage>();
        builder.Services.AddTransient<OverviewViewModel>();

        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<DetailViewModel>();

        builder.Services.AddTransient<AnalyticsPage>();
        builder.Services.AddTransient<AnalyticsViewModel>();

        builder.Services.AddTransient<OvertimePage>();
        builder.Services.AddTransient<OvertimeViewModel>();

        return builder.Build();
    }
}
