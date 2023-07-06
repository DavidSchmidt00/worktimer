using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WorkTimer.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        int time;

        [RelayCommand]
        void Start()
        {
            Time++;
        }
    }
}
