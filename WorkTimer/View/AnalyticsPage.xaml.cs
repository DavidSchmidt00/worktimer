using WorkTimer.ViewModel;

namespace WorkTimer.View;

public partial class AnalyticsPage : ContentPage
{
	public AnalyticsPage(AnalyticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
