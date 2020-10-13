using MediatR;
using System;

namespace Clockify.Core.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }
        
        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
