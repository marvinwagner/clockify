using FluentValidation;
using System;

namespace Clockify.Tracking.Domain.Models
{
    public class TimeEntryValidator : AbstractValidator<TimeEntry>
    {
        public TimeEntryValidator()
        {
            RuleFor(c => c.DayId).NotEmpty();
            RuleFor(c => c.Date).NotNull().NotEqual(DateTime.MinValue);
        }
    }
}
