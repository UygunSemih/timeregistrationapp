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
        private TimeSpan startTijd = new(9, 0, 0);

        [ObservableProperty]
        private TimeSpan eindTijd = new(15, 0, 0);

        [ObservableProperty]
        private DateTime startDatum = DateTime.Today;

        [ObservableProperty]
        private string? notitie;

        [ObservableProperty]
        private bool isHoliday; 

        private readonly TimeService timeService;

        public MainPageViewModel(TimeService ts)
        {
            timeService = ts;
        }

        [RelayCommand]
        private async void SaveTijdsRegistratie()
        {
            var registratie = new TijdsRegistratie
            {
                StartTime = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, StartTijd.Hours, StartTijd.Minutes, StartTijd.Seconds),
                EndTime = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day, EindTijd.Hours, EindTijd.Minutes, EindTijd.Seconds),
                Note = Notitie,
            };

            if (IsHoliday)
            {
                registratie.StartTime = new DateTime(StartDatum.Year, StartDatum.Month, StartDatum.Day);
                registratie.EndTime = registratie.StartTime;  
                registratie.IsHoliday = true;  
            }
            else
            {
                if (registratie.EndTime < registratie.StartTime)
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", $"End time cannot be earlier than start time!", "OK");
                    return;
                }
                else if (registratie.EndTime == registratie.StartTime)
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", $"End time cannot be equal to start time!", "OK");
                    return;
                }
            }

            timeService.AddTimeRegistration(registratie);
            await Application.Current.MainPage.DisplayAlert("Success", $"Registration added!", "OK");

            StartTijd = new(9, 0, 0);
            EindTijd = new(15, 0, 0);
            Notitie = string.Empty;
            IsHoliday = false; 
        }
    }
}
