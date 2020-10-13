using FluentValidation.Results;
using MediatR;
using System;

namespace Clockify.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}
