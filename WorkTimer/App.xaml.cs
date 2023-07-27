using WorkTimer.Database;

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
}
