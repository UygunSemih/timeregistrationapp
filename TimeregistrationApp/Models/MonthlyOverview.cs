using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Globalization;

namespace TimeregistrationApp.Models
{
    public partial class MonthlyOverview : ObservableObject
    {
        [ObservableProperty]
        private int month;

        [ObservableProperty]
        private int year;

        [ObservableProperty]
        private double totalTime;

        public override string ToString()
        {
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            var monthName = monthNames[month - 1];
            TimeSpan time = TimeSpan.FromMinutes(totalTime);
            double totalMinutes = time.TotalMinutes;
            int hours = (int)(totalMinutes / 60);
            int minutes = (int)(totalMinutes % 60);
            string totalHours = $"{hours}:{minutes.ToString().PadLeft(2, '0')}"; 
            return $"{monthName} {year} | {totalHours} {(hours == 1 ? "hour" : "hours")}";
        }
    }
}
