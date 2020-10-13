using Clockify.Core.Domain;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Clockify.Tracking.Data
{
    internal static class MediatrExtensions
    {
        public static async Task PublishEvents(this IMediator mediator, TrackingContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(e => e.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async domainEvent =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
