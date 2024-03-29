﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.View;

namespace WorkTimer.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private IDispatcherTimer timerWork;
        private IDispatcherTimer timerPause;
        private TimeSpan currentTimeWork = new TimeSpan();
        private TimeSpan currentTimePause = new TimeSpan();

        [ObservableProperty]
        string currentTimeWorkFormatted;

        [ObservableProperty]
        string currentTimePauseFormatted;

        [ObservableProperty]
        bool pauseVisible;

        [ObservableProperty]
        bool playVisible;

        [ObservableProperty]
        bool stopVisible;

        [ObservableProperty]
        bool saveVisible;
        
        private void ResetTimer() // Alle Zustands-Variablen auf Standard-Werte zurücksetzen
        {
            CurrentTimeWorkFormatted = "00:00:00";
            currentTimeWork = TimeSpan.Zero;
            CurrentTimePauseFormatted = "00:00:00";
            currentTimePause = TimeSpan.Zero;
            PlayVisible = true;
            PauseVisible = false;
            StopVisible = false;
            SaveVisible = false;
        }

        public MainViewModel()
        {
            // Standard-Werte setzen
            ResetTimer();

            // Timer für Arbeits- und Pausenzeit initialisieren und konfigurieren
            timerWork = Application.Current.Dispatcher.CreateTimer();
            timerWork.Interval = TimeSpan.FromSeconds(1);
            timerWork.Tick += (s, e) => WorkTimerTickHandler();
            timerPause = Application.Current.Dispatcher.CreateTimer();
            timerPause.Interval = TimeSpan.FromSeconds(1);
            timerPause.Tick += (s, e) => PauseTimerTickHandler();
        }

        private string FormatTime(TimeSpan timeSpan) // Hilfsmethode
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        void WorkTimerTickHandler() // Funktion wird bei jedem Tick des WorkTimers aufgerufen
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                currentTimeWork = currentTimeWork.Add(new TimeSpan(0,0,1));
                CurrentTimeWorkFormatted = FormatTime(currentTimeWork);
            });
        }

        void PauseTimerTickHandler() // Funktion wird bei jedem Tick des PauseTimers aufgerufen
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                currentTimePause = currentTimePause.Add(new TimeSpan(0, 0, 1));
                CurrentTimePauseFormatted = FormatTime(currentTimePause);
            });
        }

        [RelayCommand]
        void Start()
        {
            timerWork.Start();
            timerPause.Stop();
            PauseVisible = true;
            PlayVisible = false;
            StopVisible = true;
            SaveVisible = false;
        }

        [RelayCommand]
        void Pause()
        {
            timerWork.Stop();
            timerPause.Start();
            PauseVisible = false;
            PlayVisible = true;
        }

        [RelayCommand]
        void Stop()
        {
            timerWork.Stop();
            timerPause.Stop();
            PauseVisible = false;
            PlayVisible = true;
            StopVisible = false;
            SaveVisible = true;
        }

        [RelayCommand]
        async Task Store()
        {
            DateTime current_date = DateTime.Now.Date;
            Trace.WriteLine(string.Format("Adding Worktime for date -> {0} <-", current_date));
            // WorktimeDay Objekt mit aktuellen Werten erstellen
            WorktimeDay newWorktime = new() { Date = current_date, Absent = false, WorkTime = currentTimeWork, PauseTime = currentTimePause };
            // Erstelltes Objekt in Datenbank schreiben
            await App.WorktimeRepo.AddNewWorktimeDay(newWorktime);
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);

            ResetTimer();
            
        }

        [RelayCommand]
        async Task OpenSettings()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        [RelayCommand]
        async Task OpenOverview()
        {
            await Shell.Current.GoToAsync(nameof(OverviewPage));
        }

        [RelayCommand]
        async Task OpenAnalytics()
        {
            await Shell.Current.GoToAsync(nameof(AnalyticsPage));
        }

        [RelayCommand]
        async Task OpenOvertime()
        {
            await Shell.Current.GoToAsync(nameof(OvertimePage));
        }
    }
}
