using Clockify.Core.Domain;
using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Clockify.Tracking.Domain.Commands
{
    public class TrackingCommandHandler :
        IRequestHandler<CreatePointCommand, bool>,
        IRequestHandler<DeletePointCommand, bool>,
        IRequestHandler<CreateConfigurationCommand, bool>,
        IRequestHandler<ChangeConfigurationCommand, bool>
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IDayEntryRepository _dayEntryRepository;
        private readonly IMediator _mediator;

        public TrackingCommandHandler(IConfigurationRepository configurationRepository, IDayEntryRepository dayEntryRepository, IMediator mediator)
        {
            _configurationRepository = configurationRepository;
            _dayEntryRepository = dayEntryRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CreatePointCommand request, CancellationToken cancellationToken)
        {
            var config = await _configurationRepository.FindByUser(request.UserId);
            if (config == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "User doesn't have a configuration"));
                return false;
            }

            var day = await _dayEntryRepository.FindByDay(request.UserId, request.Date);
            if (day == null)
            {
                day = new DayEntry(request.UserId, request.Date);
                _dayEntryRepository.CreateDay(day);
            }
            var time = new TimeEntry(day.Id, request.Date);

            day.AddPoint(time, config);

            _dayEntryRepository.CreatePoint(time);
            return await _dayEntryRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(DeletePointCommand request, CancellationToken cancellationToken)
        {
            var config = await _configurationRepository.FindByUser(request.UserId);
            if (config == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "User doesn't have a configuration"));
                return false;
            }

            var point = await _dayEntryRepository.FindTimeEntry(request.TimeId);
            var day = await _dayEntryRepository.FindByDay(request.UserId, point.Date);

            day.RemovePoint(request.TimeId, config);

            _dayEntryRepository.RemovePoint(point);
            if (!day.Points.Any()) _dayEntryRepository.RemoveDay(day);

            return await _dayEntryRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
        {
            var config = await _configurationRepository.FindByUser(request.UserId);
            if (config != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "User already have a configuration"));
                return false;
            }

            config = new Configuration(request.UserId, request.LunchTime, request.ToleranceTime, request.WorkingTime);

            _configurationRepository.CreateConfiguration(config);

            return await _configurationRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(ChangeConfigurationCommand request, CancellationToken cancellationToken)
        {
            var config = await _configurationRepository.FindByUser(request.UserId);
            if (config == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Configuration doesn't exist"));
                return false;
            }

            config.ChangeLunchTime(request.LunchTime);
            config.ChangeToleranceTime(request.ToleranceTime);
            config.ChangeWorkingTime(request.WorkingTime);
            try
            {
                config.Validate();
            }
            catch (DomainException e)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, e.Message));
                return false;
            }

            return await _configurationRepository.UnitOfWork.Commit();
        }
    }
}
