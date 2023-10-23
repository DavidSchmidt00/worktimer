using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.Util;
using WorkTimer.View;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace WorkTimer.ViewModel
{
    public partial class OvertimeViewModel : ObservableObject
    {
        private readonly Task initTask;
        public OvertimeViewModel()
        {
            this.initTask = LoadWorktime();
        }

        [ObservableProperty]
        String overtimeText = "Über-/Unterstunden:";

        [ObservableProperty]
        String overtimeFormatted = "Lädt...";

        [ObservableProperty]
        Color overtimeTextColor = Colors.Black;

        async private Task LoadWorktime()
        {
            TimeSpan overtime = TimeSpan.Zero;
            // Auf Wochen aggregierte Worktime aus der Datenbank laden
            List<WorktimeAggregatedByWeek> result = await App.WorktimeRepo.GetAggregatedWorktimeByWeek();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                // Über-/Unterstunden aus den Ergebnissen der Datenbankabfrage aufsummieren
                foreach (WorktimeAggregatedByWeek day in result)
                {
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
