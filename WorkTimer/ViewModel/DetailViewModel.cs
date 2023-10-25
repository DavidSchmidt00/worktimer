using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Windows.ApplicationModel;
using WorkTimer.Models;
using WorkTimer.Util;

namespace WorkTimer.ViewModel
{
    [QueryProperty(nameof(Element), "Element")] // Übergabe-Parameter ins ViewModel
    public partial class DetailViewModel : ObservableObject
    {
        [RelayCommand]
        async Task Save()
        {
            // Wenn Übergabe-Parameter ungleich null ist, wird ein Element bearbeitet
            if (Element is not null)
            {
                WorktimeDay newWorktime = new() { Id = Element.Id, Date = DateTime.Parse(DetailDate), Absent = DetailAbsent, WorkTime = TimeSpan.Parse(DetailWorkTime), PauseTime = TimeSpan.Parse(DetailPauseTime) };
                await App.WorktimeRepo.UpdateWorktimeDay(newWorktime);
            }
            else // Wenn Übergabe-Parameter null ist, wird ein neues Element erstellt
            {
                WorktimeDay newWorktime = new() { Date = DateTime.Parse(DetailDate), Absent = DetailAbsent, WorkTime = TimeSpan.Parse(DetailWorkTime), PauseTime = TimeSpan.Parse(DetailPauseTime) };
                await App.WorktimeRepo.AddNewWorktimeDay(newWorktime);
            }
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);
            // Signal senden, dass Daten sich geändert haben (siehe OverviewViewModel)
            WeakReferenceMessenger.Default.Send(new ActionMessage("RepoUpdated"));
            await Shell.Current.GoToAsync("..");
        }

        [ObservableProperty]
        string detailDate;

        [ObservableProperty]
        string detailWorkTime;

        [ObservableProperty]
        string detailPauseTime;

        [ObservableProperty]
        bool detailAbsent;

        private WorktimeDay _element;

        public WorktimeDay Element
        {
            get { return _element; }
            set
            {
                if (value is not null)
                {
                    // Daten aus vorhandenem Element laden
                    _element = value;
                    DetailDate = _element.Date.ToString("dd.MM.yyyy");
                    DetailWorkTime = _element.WorkTime.ToString();
                    DetailPauseTime = _element.PauseTime.ToString();
                    DetailAbsent = _element.Absent;
                } else
                {
                    // Standard-Werte zur Erstellung eines neuen Eintrags laden
                    DetailDate = DateTime.Now.Date.ToString("dd.MM.yyyy");
                    DetailWorkTime = TimeSpan.Zero.ToString();
                    DetailPauseTime = TimeSpan.Zero.ToString();
                    DetailAbsent = false;
                }
            }
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
