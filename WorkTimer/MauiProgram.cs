﻿using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using WorkTimer.Database;
using WorkTimer.View;
using WorkTimer.ViewModel;

namespace WorkTimer;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        builder.Logging.AddDebug();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "worktime.db3");
        builder.Services.AddSingleton<WorktimeRepository>(s => ActivatorUtilities.CreateInstance<WorktimeRepository>(s, dbPath));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();

        builder.Services.AddTransient<OverviewPage>();
        builder.Services.AddTransient<OverviewViewModel>();

        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<DetailViewModel>();

        return builder.Build();
    }
}
