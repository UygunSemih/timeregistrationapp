using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

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

        [ObservableProperty]
        private bool isHoliday;

        public override string ToString()
        {
            string timeInfo = IsHoliday
                ? $"{StartTime.ToShortDateString()} | Holiday"
                : $"{StartTime.ToShortDateString()} | {StartTime.ToShortTimeString()} - {EndTime.ToShortTimeString()}";

            return timeInfo;
        }
    }
}
