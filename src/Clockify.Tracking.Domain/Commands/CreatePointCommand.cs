using FluentValidation;
using Clockify.Core.Messages;
using System;

namespace Clockify.Tracking.Domain.Commands
{
    public class CreatePointCommand : Command
    {
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; }

        public CreatePointCommand(Guid userId, DateTime date)
        {
            UserId = userId;
            Date = date;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreatePointCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreatePointCommandValidation : AbstractValidator<CreatePointCommand>
    {
        public CreatePointCommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid user Id");

            RuleFor(c => c.Date)
                .NotEqual(DateTime.MinValue)
                .WithMessage("Invalid date");
        }
    }
}
