using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Domain.Commands;
using Clockify.Tracking.Domain.Queries;
using Clockify.WebAPI.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Clockify.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITrackingQueries _queries;
        private readonly ILogger<PointsController> _logger;

        public PointsController(INotificationHandler<DomainNotification> notifications, IMediator mediator, ITrackingQueries queries, ILogger<PointsController> logger)
            : base(notifications)
        {
            _mediator = mediator;
            _logger = logger;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DayEntryFilterDTO filter)
        {
            var days = await _queries.LoadRange(UserId, filter.Start, filter.End);
            return Json(days);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PointDTO dto)
        {
            var command = new CreatePointCommand(UserId, dto.Time);
            await _mediator.Send(command);

            if (HasNotifications())
                return BadRequest(LoadErrorMessages());

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePointCommand(UserId, id);
            await _mediator.Send(command);

            if (HasNotifications())
                return BadRequest(LoadErrorMessages());

            return Ok();
        }
    }
}
