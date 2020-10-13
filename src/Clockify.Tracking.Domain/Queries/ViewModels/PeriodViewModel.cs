using System;
using System.Collections.Generic;

namespace Clockify.Tracking.Domain.Queries.ViewModels
{
    public class PeriodViewModel
    {
        public TimeSpan TotalExtraTime { get; set; }
        public TimeSpan TotalMissingTime { get; set; }
        public TimeSpan TotalWorkedTime { get; set; }
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
