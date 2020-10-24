using Clockify.Core.Messages.Notifications;
using Clockify.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clockify.WebAPI.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        private readonly DomainNotificationHandler _notifications;

        protected Guid UserId => GetUserId();
        protected Guid GetUserId()
        {
            return User.Identity.IsAuthenticated ? Guid.Parse(User.GetUserId()) : Guid.Empty;
        }

        protected BaseController(INotificationHandler<DomainNotification> notifications)
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