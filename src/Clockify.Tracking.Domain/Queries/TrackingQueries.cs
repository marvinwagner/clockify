using Clockify.Core.Extensions;
using Clockify.Core.Messages.Notifications;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Models;
using Clockify.Tracking.Domain.Queries.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockify.Tracking.Domain.Queries
{
    public class TrackingQueries : ITrackingQueries
    {
        private readonly IDayEntryRepository _dayEntryRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMediator _mediator;

        public TrackingQueries(IDayEntryRepository dayEntryRepository, IConfigurationRepository configurationRepository, IMediator mediator)
        {
            _dayEntryRepository = dayEntryRepository;
            _configurationRepository = configurationRepository;
            _mediator = mediator;
        }

        public async Task<ConfigurationViewModel> LoadConfiguration(Guid userId)
        {
            var config = await _configurationRepository.FindByUser(userId);
            if (config == null) return null;

            return new ConfigurationViewModel
            {
                Id = config.Id,
                LunchTime = config.LunchTime,
                ToleranceTime = config.ToleranceTime,
                WorkingTime = config.WorkingTime
            };
        }

        public async Task<PeriodViewModel> LoadRange(Guid userId, DateTime start, DateTime end)
        {
            var days = await _dayEntryRepository.FindByPeriod(userId, start, end);
            var config = await _configurationRepository.FindByUser(userId);
            if (config == null)
            {
                await _mediator.Publish(new DomainNotification("Load points", "User doesn't have a configuration"));
                return null;
            }

            var result = new PeriodViewModel();
            result.Days = new List<DayEntryViewModel>();
            foreach (var day in days)
            {
                result.Days.Add(new DayEntryViewModel
                {
                    Id = day.Id,
                    Date = day.Date,
                    ExtraTime = day.ExtraTime,
                    MissingTime = day.MissingTime,
                    WorkedTime = day.WorkedTime,
                    Points = day.Points.OrderBy(p => p.Date).Select(p => new TimeEntryViewModel
                    {
                        Id = p.Id,
                        Time = p.Date,
                        CreatedAt = p.Date
                    })
                });
            }
            var workedTime = result.Days.Sum(x => x.WorkedTime);
            var missingTime = TimeSpan.Zero;
            var extraTime = TimeSpan.Zero;
            
            var totalHours = config.WorkingTime * GetWorkingDays(start, end);
            result.TotalWorkHours = string.Format("{0}hr {1}m",
                     (int)totalHours.TotalHours,
                     totalHours.Minutes);

            if (workedTime > totalHours)
                extraTime = workedTime.Subtract(totalHours);
            else if (totalHours > workedTime)
                missingTime = totalHours.Subtract(workedTime);
                
            result.TotalExtraTime = FormatTimespan(extraTime);
            result.TotalMissingTime = FormatTimespan(missingTime);
            result.TotalWorkedTime = FormatTimespan(workedTime);

            result.WorkedTimePercentage = (result.Days.Sum(x => x.WorkedTime).TotalHours * 100) 
                / totalHours.TotalHours;
            result.BankTimePercentage = ((result.BankSign > 0 ? extraTime.TotalHours : missingTime.TotalHours) * 100) 
                / totalHours.TotalHours;

            if (missingTime.Ticks > 0)
                result.BankSign = -1;
            else if (extraTime.Ticks > 0)
                result.BankSign = 1;

            return result;
        }

        private string FormatTimespan(TimeSpan span)
        {
            return $"{(int)span.TotalHours}:{span.Minutes}";
        }

        private int GetWorkingDays(DateTime start, DateTime end)
        {
            int days = 0;
            var day = start.Date;
            while (day <= end.Date)
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                    days++;
                day = day.AddDays(1);
            }
            return days;
        }
    }
}
