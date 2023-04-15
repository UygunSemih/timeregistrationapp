using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TimeregistrationApp.Models;
using TimeregistrationApp.Services;

#nullable enable
namespace TimeregistrationApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private TimeSpan startTijd;
        [ObservableProperty]
        private TimeSpan eindTijd;
        [ObservableProperty]
        private DateTime startDatum = DateTime.Today;
        [ObservableProperty]
        private string? notitie;

        private TimeService timeService;

        public MainPageViewModel(TimeService ts)
        {
            timeService= ts;
        }

        [RelayCommand]
        private async void SaveTijdsRegistratie()
        {
            var registratie = new TijdsRegistratie();
            var start = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, StartTijd.Hours, StartTijd.Minutes, StartTijd.Seconds);
            var end = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, EindTijd.Hours, EindTijd.Minutes, EindTijd.Seconds);
            if(end < start)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", $"End time can not be smaller than start time!", "OK");
            }
            else if (end == start)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", $"End time can not be equal to start time!", "OK");
            }
            else
            {
                registratie.StartTime = start;
                registratie.EndTime = end;
                registratie.Note = Notitie;
                timeService.AddTimeRegistration(registratie);

                await Application.Current.MainPage.DisplayAlert("Success", $"{registratie} toegevoegd!", "OK");

                StartTijd = TimeSpan.Zero;
                EindTijd = TimeSpan.Zero;
                Notitie = string.Empty;
            }          
        }
    }
}
