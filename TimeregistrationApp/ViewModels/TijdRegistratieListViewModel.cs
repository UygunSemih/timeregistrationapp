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
        private int selectedMaandIndex;

        [ObservableProperty]
        private string downloadButtonText = "Download PDF";

        public ObservableCollection<string> MonthPickerItems { get; set; }

        private readonly TimeService timeService;

        private readonly Dictionary<int, DateTime> monthYearMap;

        public TijdRegistratieListViewModel(TimeService ts)
        {
            timeService = ts;
            AllTijdRegistraties = new ObservableCollection<TijdsRegistratie>();
            registraties = new List<TijdsRegistratie>();
            MonthPickerItems = new ObservableCollection<string>();
            monthYearMap = new Dictionary<int, DateTime>();

            LoadTijdregistraties();
        }

        private void PopulateMonthPickerItems()
        {
            MonthPickerItems.Clear();
            monthYearMap.Clear();
            MonthPickerItems.Add("All");
            int index = 1;
            int defaultIndex = 0;

            var groupedRegistraties = registraties
                .GroupBy(r => new { r.StartTime.Year, r.StartTime.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month);

            foreach (var group in groupedRegistraties)
            {
                string monthYear = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month)} {group.Key.Year}";
                MonthPickerItems.Add(monthYear);
                DateTime date = new(group.Key.Year, group.Key.Month, 1);
                monthYearMap.Add(index, date);

                if (group.Key.Year == DateTime.Now.Year && group.Key.Month == DateTime.Now.Month)
                {
                    defaultIndex = index;
                }

                index++;
            }

            SelectedMaandIndex = defaultIndex;

            OnPropertyChanged(nameof(MonthPickerItems));
        }

        [RelayCommand]
        private void OnMonthSelected()
        {
            if (selectedMaandIndex > 0 && monthYearMap.ContainsKey(selectedMaandIndex))
            {
                DateTime selectedDate = monthYearMap[selectedMaandIndex];
                var filteredRegistraties = registraties
                    .Where(r => r.StartTime.Month == selectedDate.Month && r.StartTime.Year == selectedDate.Year)
                    .OrderBy(x => x.EndTime);

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
            UpdateDagenCountString();
        }

        private void UpdateDagenCountString()
        {
            int aantalDagen = AllTijdRegistraties.Where(r => !r.IsHoliday).Count();
            int verlofDagen = AllTijdRegistraties.Count - aantalDagen;

            string totalDaysText = $"{aantalDagen} {(aantalDagen == 1 ? "day" : "days")}";

            string vacationDaysText = verlofDagen > 0
                ? $"{verlofDagen} {(verlofDagen == 1 ? "day" : "days")} holiday"
                : string.Empty;

            string monthText = (selectedMaandIndex >= 0 && selectedMaandIndex < MonthPickerItems.Count)
                ? MonthPickerItems[selectedMaandIndex]
                : "All";

            DagenCountString = $"Total {totalDaysText}{(string.IsNullOrEmpty(vacationDaysText) ? string.Empty : " | " + vacationDaysText)} | {monthText}";
        }

        [RelayCommand]
        public async void GeneratePDF()
        {
            string titel = $"Overview {MonthPickerItems[selectedMaandIndex]} - {DateTime.Now.Year}";
            if (selectedMaandIndex > 0 && monthYearMap.ContainsKey(selectedMaandIndex))
            {
                DateTime selectedDate = monthYearMap[selectedMaandIndex];
                await PDFGenerator.GeneratePDF(
                    registraties.Where(r => r.StartTime.Month == selectedDate.Month && r.StartTime.Year == selectedDate.Year)
                                .OrderBy(x => x.EndTime).ToList(), titel);
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
            PopulateMonthPickerItems();
            OnMonthSelected();
        }

        [RelayCommand]
        public void VerwijderRegistratie(TijdsRegistratie tijdsRegistratie)
        {
            timeService.DeleteTimeRegistrationAsync(tijdsRegistratie);

            registraties.Remove(tijdsRegistratie);
            AllTijdRegistraties.Remove(tijdsRegistratie);
            DagenCountString = $"Total {AllTijdRegistraties.Count} {(AllTijdRegistraties.Count == 1 ? "day" : "days")} | {MonthPickerItems[selectedMaandIndex]}";
            PopulateMonthPickerItems();
            UpdateDagenCountString();
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
