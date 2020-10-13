using Clockify.Core.Messages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clockify.WebAPI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;

        protected Guid UserId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32"); // TODO get from auth service

        protected ControllerBase(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        protected bool HasNotifications()
        {
            return _notifications.HasNotification();
        }

        protected IEnumerable<string> LoadErrorMessages()
        {
            return _notifications.GetNotifications().Select(c => c.Value).ToList();
        }
    }
}