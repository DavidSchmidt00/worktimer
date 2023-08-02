﻿using WorkTimer.Database;

namespace WorkTimer;

public partial class App : Application
{
    public static WorktimeRepository WorktimeRepo { get; private set; }
    public App(WorktimeRepository repo)
	{
		InitializeComponent();

		MainPage = new AppShell();

		WorktimeRepo = repo;
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 800;
        const int newHeight = 600;

        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
}
