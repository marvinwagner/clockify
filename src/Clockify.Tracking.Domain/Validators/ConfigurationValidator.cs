using FluentValidation;
using System;

namespace Clockify.Tracking.Domain.Models
{
    public class ConfigurationValidator : AbstractValidator<Configuration>
    {
        public ConfigurationValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.LunchTime).NotEqual(TimeSpan.Zero);
            RuleFor(c => c.ToleranceTime).NotEqual(TimeSpan.Zero);
            RuleFor(c => c.WorkingTime).NotEqual(TimeSpan.Zero);

            // Preventing absurds
            RuleFor(c => c.WorkingTime).GreaterThan(c => c.LunchTime);
            RuleFor(c => c.ToleranceTime).LessThan(c => c.WorkingTime);
        }
    }
}
