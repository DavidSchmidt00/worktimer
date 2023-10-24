using WorkTimer.ViewModel;

namespace WorkTimer.View;

public partial class OvertimePage : ContentPage
{
	public OvertimePage(OvertimeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
