using WorkTimer.ViewModel;

namespace WorkTimer;

public partial class OvertimePage : ContentPage
{
	public OvertimePage(OvertimeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
