using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.Util;
using WorkTimer.View;

namespace WorkTimer.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        public float settingsWorktimeWeeklyHours;

        [ObservableProperty]
        public int settingsVacationDays;

        [ObservableProperty]
        public bool settingsStandingDesk;

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task Save()
        {
            Settings newSettings = new() { Id = 1, WorktimeWeeklyHours = SettingsWorktimeWeeklyHours, VacationDays = SettingsVacationDays, StandingDesk = SettingsStandingDesk };
            await App.WorktimeRepo.UpdateSettings(newSettings);
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);
        }

        private readonly Task initTask;
        public SettingsViewModel()
        {
            this.initTask = LoadWorktime();
        }

        async private Task LoadWorktime()
        {
            Settings settings = await App.WorktimeRepo.GetSettings();
            Trace.WriteLine("Got settings");
            if (settings is not null)
            {
                SettingsWorktimeWeeklyHours = settings.WorktimeWeeklyHours;
                SettingsVacationDays = settings.VacationDays;
                SettingsStandingDesk = settings.StandingDesk;
            }
        }
    }
}
