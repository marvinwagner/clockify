using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Data;
using Clockify.Tracking.Data.Repository;
using Clockify.Tracking.Domain.Commands;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Clockify.WebAPI.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Notification
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Tracking
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IDayEntryRepository, DayEntryRepository>();
            services.AddScoped<TrackingContext>();

            services.AddScoped<ITrackingQueries, TrackingQueries>();

            services.AddScoped<IRequestHandler<CreatePointCommand, bool>, TrackingCommandHandler>();
            services.AddScoped<IRequestHandler<DeletePointCommand, bool>, TrackingCommandHandler>();
            services.AddScoped<IRequestHandler<CreateConfigurationCommand, bool>, TrackingCommandHandler>();
            services.AddScoped<IRequestHandler<ChangeConfigurationCommand, bool>, TrackingCommandHandler>();

        }
    }
}
