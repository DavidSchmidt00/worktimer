using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.Util;
using WorkTimer.View;

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
            WeakReferenceMessenger.Default.Register<ActionMessage>(this, async (r, m) =>
            {
                await LoadWorktime();
            });
        }

        async private Task LoadWorktime()
        {
            List<WorktimeDay> result = await App.WorktimeRepo.ListAllWorktime();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                WorktimeList.Clear();
                foreach (WorktimeDay day in result) { WorktimeList.Add(day); }
            }
        }

        [RelayCommand]
        async Task Delete(WorktimeDay element)
        {
            await App.WorktimeRepo.DeleteWorktimeDay(element);
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);
            await LoadWorktime();
        }

        [RelayCommand]
        async Task OpenDetail(WorktimeDay element)
        {
            var param = new Dictionary<string, object> { { "Element", element } };
            await Shell.Current.GoToAsync(nameof(DetailPage), param);
        }


        [RelayCommand]
        async Task Add()
        {
            var param = new Dictionary<string, object> { { "Element", null } };
            await Shell.Current.GoToAsync(nameof(DetailPage), param);
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
