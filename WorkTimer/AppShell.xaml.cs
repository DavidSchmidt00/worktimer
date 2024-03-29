﻿using WorkTimer.View;

namespace WorkTimer;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        // Alle Seiten als Routen registrieren
		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(OverviewPage), typeof(OverviewPage));
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(AnalyticsPage), typeof(AnalyticsPage));
        Routing.RegisterRoute(nameof(OvertimePage), typeof(OvertimePage));
    }
}
