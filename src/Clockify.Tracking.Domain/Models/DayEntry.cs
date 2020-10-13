using Clockify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clockify.Tracking.Domain.Models
{
    public class DayEntry : Entity<DayEntry>, IAggregateRoot
    {
        private readonly List<TimeEntry> _points;
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan ExtraTime { get; private set; }
        public TimeSpan MissingTime { get; private set; }
        public TimeSpan WorkedTime { get; private set; }
        public IReadOnlyCollection<TimeEntry> Points => _points;

        protected DayEntry() { }

        public DayEntry(Guid userId, DateTime date)
        {
            UserId = userId;
            Date = date;
            _points = new List<TimeEntry>();

            Validate(this, new DayEntryValidator());
        }

        public void AddPoint(TimeEntry timeEntry, Configuration config)
        {
            _points.Add(timeEntry);

            CalculateTimes(config);
        }

        public void RemovePoint(Guid timeId, Configuration config)
        {
            var point = _points.FirstOrDefault(t => t.Id == timeId);
            if (point != null)
                _points.Remove(point);

            CalculateTimes(config);
        }

        public void CalculateTimes(Configuration config)
        {
            if (_points.Count > 0 && _points.Count % 2 == 0)
            {
                WorkedTime = SumWorkedTime();
                CalculateExtraTime(config);
                CalculateMissingTime(config);
                CalculateDefaultTimes(config);
            }
            else
            {
                ExtraTime = TimeSpan.Zero;
                MissingTime = TimeSpan.Zero;
            }
        }

        private TimeSpan SumWorkedTime()
        {
            var workedTime = TimeSpan.Zero;
            var sortedPoints = _points.OrderBy(x => x.Date).ToList();
            for (int i = 0; i < sortedPoints.Count; i += 2)
            {
                workedTime = workedTime.Add(DifferenceFrom(sortedPoints[i].Date, sortedPoints[i + 1].Date));
            }
            return workedTime;
        }

        private void CalculateDefaultTimes(Configuration config)
        {
            if (WorkedTime >= config.WorkingTime.Subtract(config.ToleranceTime) &&
                    WorkedTime <= config.WorkingTime.Add(config.ToleranceTime))
            {
                ExtraTime = TimeSpan.Zero;
                MissingTime = TimeSpan.Zero;
            }
        }

        private void CalculateExtraTime(Configuration config)
        {
            if (WorkedTime > config.WorkingTime.Add(config.ToleranceTime))
            {
                ExtraTime = WorkedTime.Subtract(config.WorkingTime);
            }
        }

        private void CalculateMissingTime(Configuration config)
        {
            if (WorkedTime < config.WorkingTime.Subtract(config.ToleranceTime))
            {
                MissingTime = config.WorkingTime.Subtract(WorkedTime);
            }
        }

        private TimeSpan DifferenceFrom(DateTime t1, DateTime t2)
        {
            return TimeSpan.FromTicks(t2.Subtract(t1).Ticks);
        }
    }
}
