using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeregistrationApp.Models;
using TimeregistrationApp.Services;
using TimeregistrationApp.Views;

namespace TimeregistrationApp.ViewModels
{
    public partial class TijdRegistratieListViewModel : ObservableObject
    {
        public ObservableCollection<TijdsRegistratie> AllTijdRegistraties { get; set; }

        private List<TijdsRegistratie> registraties;

        [ObservableProperty]
        private string dagenCountString;

        [ObservableProperty]
        private static int selectedMaandIndex = DateTime.Now.Month;

        [ObservableProperty]
        private string downloadButtonText = "Download PDF";

        private TimeService timeService;

        public TijdRegistratieListViewModel(TimeService ts)
        {
            timeService = ts;
            AllTijdRegistraties = new ObservableCollection<TijdsRegistratie>();
            registraties = new List<TijdsRegistratie>();
        }

        public static string getFullName(int month)
        {
            if(month != 0)
            {
                DateTime date = new DateTime(2020, month, 1);
                CultureInfo englishCulture = CultureInfo.CreateSpecificCulture("en-US");
                return date.ToString("MMMM", englishCulture);
            }
            return "All";            
        }

        [RelayCommand]
        private void OnMonthSelected()
        {
            if (selectedMaandIndex != 0)
            {
                var filteredRegistraties = registraties.Where(r => r.StartTime.Month == selectedMaandIndex).OrderBy(x => x.EndTime);
                AllTijdRegistraties.Clear();
                foreach (var registratie in filteredRegistraties)
                {
                    AllTijdRegistraties.Add(registratie);
                }
            }
            else
            {
                AllTijdRegistraties.Clear();
                foreach (var registratie in registraties.OrderBy(x => x.EndTime))
                {
                    AllTijdRegistraties.Add(registratie);
                }
            }
            DagenCountString = $"Total {AllTijdRegistraties.Count()} {(AllTijdRegistraties.Count == 1 ? "day" : "days")} | {getFullName(selectedMaandIndex)}";
        }

        [RelayCommand]
        public async void GeneratePDF()
        {
            string titel = $"Timeregistration Overview {getFullName(selectedMaandIndex)} - {DateTime.Now.Year}";
            if (selectedMaandIndex != 0)
            {
                await PDFGenerator.GeneratePDF(registraties.Where(r => r.StartTime.Month == selectedMaandIndex).OrderBy(x => x.EndTime).ToList(), titel);
            }
            else
            {
                await PDFGenerator.GeneratePDF(registraties.OrderBy(x => x.EndTime).ToList(), titel);
            }
                
        }

        [RelayCommand]
        public async void LoadTijdregistraties()
        {
            registraties = await timeService.GetAllTimeRegistrations();
            OnMonthSelected();
        }

        [RelayCommand]
        public void VerwijderRegistratie(TijdsRegistratie tijdsRegistratie)
        {
            timeService.DeleteTimeRegistrationAsync(tijdsRegistratie);

            registraties.Remove(tijdsRegistratie);
            AllTijdRegistraties.Remove(tijdsRegistratie);
            DagenCountString = $"Total {AllTijdRegistraties.Count()} {(AllTijdRegistraties.Count == 1 ? "day" : "days")} | {getFullName(selectedMaandIndex)}";
        }

        [RelayCommand]
        public async void GoToDetail(TijdsRegistratie clickedTijdsRegistratie)
        {
            await Shell.Current.GoToAsync(nameof(TijdRegistratieDetailView), true, new Dictionary<string, object>
            {
                {"selectedTijdsRegistratie", clickedTijdsRegistratie}
            });
        }
    }
}
