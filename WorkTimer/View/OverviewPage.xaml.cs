using WorkTimer.ViewModel;

namespace WorkTimer.View;

public partial class OverviewPage : ContentPage
{
	public OverviewPage(OverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}