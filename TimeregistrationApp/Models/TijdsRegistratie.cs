using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace TimeregistrationApp.Models
{
    public partial class TijdsRegistratie : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ObservableProperty]
        private DateTime startTime;
        [ObservableProperty]
        private DateTime endTime;
        [ObservableProperty]
        private string? note;

        public override string ToString()
        {
            return $"{startTime.ToShortDateString()} | {startTime.ToShortTimeString()} - {endTime.ToShortTimeString()}";
        }
    }
}
