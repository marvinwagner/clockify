using Clockify.Core.Domain;
using System;

namespace Clockify.Tracking.Domain.Models
{
    public class Configuration : Entity<Configuration>, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public TimeSpan LunchTime { get; private set; }
        public TimeSpan ToleranceTime { get; private set; }
        public TimeSpan WorkingTime { get; private set; }

        protected Configuration() { }

        public Configuration(Guid userId, TimeSpan lunchTime, TimeSpan toleranceTime, TimeSpan workingTime)
        {
            UserId = userId;
            LunchTime = lunchTime;
            ToleranceTime = toleranceTime;
            WorkingTime = workingTime;

            Validate(this, new ConfigurationValidator());
        }

        public void ChangeLunchTime(TimeSpan lunchTime)
        {
            LunchTime = lunchTime;
        }

        public void ChangeToleranceTime(TimeSpan toleranceTime)
        {
            ToleranceTime = toleranceTime;
        }

        public void ChangeWorkingTime(TimeSpan workingTime)
        {
            WorkingTime = workingTime;
        }

        public void Validate()
        {
            Validate(this, new ConfigurationValidator());
        }
    }
}
