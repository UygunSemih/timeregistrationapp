using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private TimeService timeService;

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
            .GroupBy(r => r.StartTime.Month)
            .Select(g => new MonthlyOverview
            {
                Month = g.Key,
                TotalTime = g.Sum(r => (r.EndTime - r.StartTime).TotalMinutes)
            });

            AllMonthlyOverviews.Clear();
            foreach(var s in groupedRegistraties)
            {
                AllMonthlyOverviews.Add(s);
            }
        }
    }
}
