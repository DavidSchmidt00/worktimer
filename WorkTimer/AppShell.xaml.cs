﻿namespace WorkTimer;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}
