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
            // Handler auf eine Nachricht vom Typ "ActionMessage" registrieren, der dann die Daten neu aus der Datenbank lädt
            WeakReferenceMessenger.Default.Register<ActionMessage>(this, async (r, m) =>
            {
                await LoadWorktime();
            });
        }

        async private Task LoadWorktime()
        {
            WorktimeList.Clear();
            // Alle Einträge aus der Datenbank laden
            List<WorktimeDay> result = await App.WorktimeRepo.ListAllWorktime();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                // Ergebnisse nach Datum sortieren
                result = result.OrderBy(e => e.Date).ToList();
                // Ergebnisse der Datenbankabfrage in Liste übernehmen
                foreach (WorktimeDay day in result) { WorktimeList.Add(day); }
            }
        }

        [RelayCommand]
        async Task Delete(WorktimeDay element) // Ausgewähltes Element aus Datenbank löschen
        {
            await App.WorktimeRepo.DeleteWorktimeDay(element);
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);
            // Daten neu laden
            await LoadWorktime();
        }

        [RelayCommand]
        async Task OpenDetail(WorktimeDay element)
        {
            // DetailPage mit dem ausgewählten Element als Übergabeparamter aufrufen
            var param = new Dictionary<string, object> { { "Element", element } };
            await Shell.Current.GoToAsync(nameof(DetailPage), param);
        }


        [RelayCommand]
        async Task Add()
        {
            // DetailPage mit leerem Übergabeparamter aufrufen
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
