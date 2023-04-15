using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeregistrationApp.Models;
using TimeregistrationApp.Services;

#nullable enable
namespace TimeregistrationApp.ViewModels
{
    [QueryProperty("Tijdsregistratie", "selectedTijdsRegistratie")]
    public partial class TijdRegistratieDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private TimeSpan startTijd;
        [ObservableProperty]
        private TimeSpan eindTijd;
        [ObservableProperty]
        private DateTime startDatum;
        [ObservableProperty]
        private string? notitie;

        private TimeService timeService;
        public TijdRegistratieDetailViewModel(TimeService ts)
        {
            timeService = ts;
        }

        private TijdsRegistratie tijdsRegistratie;
        public TijdsRegistratie Tijdsregistratie { set
            {
                tijdsRegistratie = value;
                StartTijd = tijdsRegistratie.StartTime.TimeOfDay;
                EindTijd = tijdsRegistratie.EndTime.TimeOfDay;
                StartDatum = tijdsRegistratie.StartTime.Date;
                Notitie = tijdsRegistratie.Note;
            } 
        }

        [RelayCommand]
        private async void UpdateTijdsRegistratie()
        {
            var start = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, StartTijd.Hours, StartTijd.Minutes, StartTijd.Seconds);
            var end = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, EindTijd.Hours, EindTijd.Minutes, EindTijd.Seconds);
            tijdsRegistratie.StartTime = start;
            tijdsRegistratie.EndTime = end;
            tijdsRegistratie.Note = notitie;
            var result = await Application.Current.MainPage.DisplayAlert("Bevestiging nodig!", $"{tijdsRegistratie} updaten?", "OK!", "Annuleer");

            if (result)
            {
                timeService.AddTimeRegistration(tijdsRegistratie);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
