using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimeregistrationApp.Models
{
    public partial class MonthlyOverview : ObservableObject
    {
        [ObservableProperty]
        private int month;

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
            double totaal = (totalMinutes / 60) * 11;
            string totalHours = $"{hours}:{minutes.ToString().PadRight(2,'0')}";
            return $"{monthName} | {totalHours} {(hours == 1 ? "hour" : "hours")}";
            //return $"{monthName} | {totalHours} uur | {totaal.ToString("C", new CultureInfo("nl-NL"))}";
        }

    }

}
