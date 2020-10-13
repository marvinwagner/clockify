using FluentValidation;
using Clockify.Core.Messages;
using System;

namespace Clockify.Tracking.Domain.Commands
{
    public class DeletePointCommand : Command
    {
        public Guid UserId { get; private set; }
        public Guid TimeId { get; private set; }

        public DeletePointCommand(Guid userId, Guid timeId)
        {
            UserId = userId;
            TimeId = timeId;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeletePointCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeletePointCommandValidation : AbstractValidator<DeletePointCommand>
    {
        public DeletePointCommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid user Id");

            RuleFor(c => c.TimeId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid time id");
        }
    }
}
