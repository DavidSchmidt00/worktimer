using WorkTimer.Database;

namespace WorkTimer;

public partial class App : Application
{
    public static WorktimeRepository WorktimeRepo { get; private set; }
    public App(WorktimeRepository worktimeRepo)
	{
		InitializeComponent();

		MainPage = new AppShell();

		WorktimeRepo = worktimeRepo;
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        // Fenster wird auf eine fixe Größe beim Start gesetzt
        const int newWidth = 800;
        const int newHeight = 600;

        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
}
