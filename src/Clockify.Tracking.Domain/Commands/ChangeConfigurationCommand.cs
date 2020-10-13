﻿using FluentValidation;
using Clockify.Core.Messages;
using System;

namespace Clockify.Tracking.Domain.Commands
{
    public class ChangeConfigurationCommand : Command
    {
        public Guid UserId { get; private set; }
        public TimeSpan LunchTime { get; private set; }
        public TimeSpan ToleranceTime { get; private set; }
        public TimeSpan WorkingTime { get; private set; }

        public ChangeConfigurationCommand(Guid userId, TimeSpan lunchTime, TimeSpan toleranceTime, TimeSpan workingTime)
        {
            UserId = userId;
            LunchTime = lunchTime;
            ToleranceTime = toleranceTime;
            WorkingTime = workingTime;
        }

        public override bool IsValid()
        {
            ValidationResult = new ChangeConfigurationCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ChangeConfigurationCommandValidation : AbstractValidator<ChangeConfigurationCommand>
    {
        public ChangeConfigurationCommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid user Id");

            RuleFor(c => c.LunchTime)
                .NotEqual(TimeSpan.Zero)
                .WithMessage("Lunch time can't be zero");

            RuleFor(c => c.WorkingTime)
                .NotEqual(TimeSpan.Zero)
                .WithMessage("Working time can't be zero");
        }
    }
}
