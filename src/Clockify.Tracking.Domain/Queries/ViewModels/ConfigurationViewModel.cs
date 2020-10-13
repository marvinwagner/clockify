using System;

namespace Clockify.Tracking.Domain.Queries.ViewModels
{
    public class ConfigurationViewModel
    {
        public Guid Id { get; set; }
        public TimeSpan LunchTime { get; set; }
        public TimeSpan ToleranceTime { get; set; }
        public TimeSpan WorkingTime { get; set; }
    }
}
