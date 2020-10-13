using FluentValidation;
using System;

namespace Clockify.Tracking.Domain.Models
{
    public class DayEntryValidator : AbstractValidator<DayEntry>
    {
        public DayEntryValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Date).NotNull().NotEqual(DateTime.MinValue);
        }
    }
}
