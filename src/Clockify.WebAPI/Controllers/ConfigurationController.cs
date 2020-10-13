using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Domain.Commands;
using Clockify.Tracking.Domain.Queries;
using Clockify.Tracking.Domain.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockify.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITrackingQueries _queries;
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(INotificationHandler<DomainNotification> notifications, IMediator mediator, ILogger<ConfigurationController> logger, ITrackingQueries queries)
            : base(notifications)
        {
            _mediator = mediator;
            _logger = logger;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _queries.LoadConfiguration(UserId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConfigurationViewModel model)
        {
            var command = new CreateConfigurationCommand(UserId, model.LunchTime, model.ToleranceTime, model.WorkingTime);
            await _mediator.Send(command);

            if (HasNotifications())
                return BadRequest(LoadErrorMessages());

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ConfigurationViewModel model)
        {
            var command = new ChangeConfigurationCommand(UserId, model.LunchTime, model.ToleranceTime, model.WorkingTime);
            await _mediator.Send(command);

            if (HasNotifications())
                return BadRequest(LoadErrorMessages());

            return Ok();
        }
    }
}
