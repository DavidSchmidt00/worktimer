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
    [QueryProperty("Element", "Element")]
    public partial class DetailViewModel : ObservableObject
    {
        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task Save()
        {
            WorktimeDay newWorktime = new() { Id = Element.Id, Date = DateTime.Parse(DetailDate), Absent = DetailAbsent, WorkTime = TimeSpan.Parse(DetailWorkTime), PauseTime = TimeSpan.Parse(DetailPauseTime) };
            await App.WorktimeRepo.UpdateWorktimeDay(newWorktime);
            string statusMessage = App.WorktimeRepo.StatusMessage;
            Trace.WriteLine(statusMessage);
            WeakReferenceMessenger.Default.Send(new ActionMessage("ElementUpdated"));
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

        public WorktimeDay Element {
            get { return _element; }
            set 
            { 
                _element = value;
                DetailDate = _element.Date.ToString("dd.MM.yyyy");
                DetailWorkTime = _element.WorkTime.ToString();
                DetailPauseTime = _element.PauseTime.ToString();
                DetailAbsent = _element.Absent;
            }
        }
    }
}
