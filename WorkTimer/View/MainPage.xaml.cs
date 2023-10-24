using WorkTimer.ViewModel;

namespace WorkTimer.View;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}

