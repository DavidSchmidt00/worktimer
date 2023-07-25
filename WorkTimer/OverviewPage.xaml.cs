using WorkTimer.ViewModel;

namespace WorkTimer;

public partial class OverviewPage : ContentPage
{
	public OverviewPage(OverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}