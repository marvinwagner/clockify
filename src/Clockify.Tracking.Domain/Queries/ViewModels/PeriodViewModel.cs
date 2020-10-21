using System;
using System.Collections.Generic;

namespace Clockify.Tracking.Domain.Queries.ViewModels
{
    public class PeriodViewModel
    {
        public string TotalWorkHours { get; set; }
        public string TotalExtraTime { get; set; }
        public string TotalMissingTime { get; set; }
        public string TotalWorkedTime { get; set; }
        public double WorkedTimePercentage { get; set; }
        public double BankTimePercentage { get; set; }
        public int BankSign { get; set; }
        public List<DayEntryViewModel> Days { get; set; }
    }

    public class DayEntryViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ExtraTime { get; set; }
        public TimeSpan MissingTime { get; set; }
        public TimeSpan WorkedTime { get; set; }
        public IEnumerable<TimeEntryViewModel> Points { get; set; }
    }

    public class TimeEntryViewModel
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
