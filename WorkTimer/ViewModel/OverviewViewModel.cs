using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkTimer.Models;

namespace WorkTimer.ViewModel
{
    [QueryProperty("WorktimeList", "WorktimeList")]
    public partial class OverviewViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<WorktimeDay> worktimeList = new ObservableCollection<WorktimeDay>();

        private readonly Task initTask;

        public OverviewViewModel() 
        {
            this.initTask = LoadWorktime();
        }


        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        async private Task LoadWorktime()
        {
            List<WorktimeDay> result = await App.WorktimeRepo.GetAllWorktime();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                WorktimeList.Clear();
                foreach (WorktimeDay day in result) { WorktimeList.Add(day); }
            }
        }
    }
}
