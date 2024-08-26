using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TimeregistrationApp.Models;
using TimeregistrationApp.Services;

namespace TimeregistrationApp.ViewModels
{
    public partial class MaandTotalenListViewModel : ObservableObject
    {
        public ObservableCollection<TijdsRegistratie> AllTijdRegistraties { get; set; }

        [ObservableProperty]
        private int selectedMaandIndex = -1;

        public ObservableCollection<MonthlyOverview> AllMonthlyOverviews { get; set; }

        private readonly TimeService timeService;

        public MaandTotalenListViewModel(TimeService timeService)
        {
            this.timeService = timeService;
            AllTijdRegistraties = new ObservableCollection<TijdsRegistratie>();
            AllMonthlyOverviews = new ObservableCollection<MonthlyOverview>();
        }

        [RelayCommand]
        public async void GroupByMonth()
        {
            var registraties = await timeService.GetAllTimeRegistrations();
            AllTijdRegistraties.Clear();

            foreach (var registratie in registraties)
            {
                AllTijdRegistraties.Add(registratie);
            }

            var groupedRegistraties = AllTijdRegistraties
                .GroupBy(r => new { r.StartTime.Year, r.StartTime.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyOverview
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalTime = g.Sum(r => (r.EndTime - r.StartTime).TotalMinutes)
                });

            AllMonthlyOverviews.Clear();
            foreach (var s in groupedRegistraties)
            {
                AllMonthlyOverviews.Add(s);
            }
        }
    }
}
