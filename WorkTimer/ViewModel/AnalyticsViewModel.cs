using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.Util;
using WorkTimer.View;

namespace WorkTimer.ViewModel
{
    public partial class AnalyticsViewModel : ObservableObject
    {
        private readonly Task initTask;
        public AnalyticsViewModel()
        {
            this.initTask = LoadWorktime();
        }

        [ObservableProperty]
        ObservableCollection<WorktimeAggregatedByWeek> worktimeAggregatedList = new ObservableCollection<WorktimeAggregatedByWeek>();

        [ObservableProperty]
        String overtimeText = "Über-/Unterstunden:";

        [ObservableProperty]
        String overtimeFormatted = "Lädt...";

        [ObservableProperty]
        Color overtimeTextColor = Colors.Black;

        async private Task LoadWorktime()
        {
            TimeSpan overtime = TimeSpan.Zero;
            List<WorktimeAggregatedByWeek> result = await App.WorktimeRepo.GetAggregatedWorktimeByWeek();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                WorktimeAggregatedList.Clear();
                foreach (WorktimeAggregatedByWeek day in result)
                {
                    WorktimeAggregatedList.Add(day);
                    overtime = overtime.Add(day.Overtime);
                }
            }
            OvertimeFormatted = overtime.ToString();
            if (overtime >= TimeSpan.Zero)
            {
                OvertimeText = "Überstunden:";
                OvertimeTextColor = Colors.Green;
            }else
            {
                OvertimeText = "Unterstunden:";
                OvertimeTextColor = Colors.Red;
            }
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
