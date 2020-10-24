using Clockify.Core.Domain;
using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Data;
using Clockify.Tracking.Data.Repository;
using Clockify.Tracking.Domain.Commands;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Queries;
using Clockify.WebAPI.Extensions;
using KissLog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Clockify.WebAPI.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // KissLog
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILogger>((context) =>
            {
                return Logger.Factory.Get();
            });

            // Auth
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            // Swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

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
