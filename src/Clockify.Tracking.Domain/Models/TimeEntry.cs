using Clockify.Core.Domain;
using System;

namespace Clockify.Tracking.Domain.Models
{
    public class TimeEntry : Entity<TimeEntry>
    {
        public Guid DayId { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime CreatedAt { get; private set; }
        // EF
        public DayEntry Day { get; private set; }

        public TimeEntry(Guid dayId, DateTime date)
        {
            DayId = dayId;
            Date = date;
            CreatedAt = DateTime.Now;
        }
    }
}
