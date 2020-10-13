using Clockify.Core.Messages;
using System.Collections.Generic;

namespace Clockify.Core.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<Event> Notifications { get; }
        void ClearEvents();
    }
}
