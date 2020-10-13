using Clockify.Core.Extensions;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Queries.ViewModels;
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

        public TrackingQueries(IDayEntryRepository dayEntryRepository, IConfigurationRepository configurationRepository)
        {
            _dayEntryRepository = dayEntryRepository;
            _configurationRepository = configurationRepository;
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

        public async Task<PeriodViewModel> LoadMonth(Guid userId, DateTime start, DateTime end)
        {
            var days = await _dayEntryRepository.FindByPeriod(userId, start, end);

            var result = new PeriodViewModel();
            result.Days = new List<DayEntryViewModel>();
            foreach (var day in days)
            {
                result.Days.Add(new DayEntryViewModel
                {
                    Id = day.Id,
                    Date = day.Date,//.ToString("yyyy-MM-dd"),
                    ExtraTime = day.ExtraTime,//.ToString(@"hh\:mm"),
                    MissingTime = day.MissingTime,//.ToString(@"hh\:mm"),
                    Points = day.Points.Select(p => new TimeEntryViewModel
                    {
                        Id = p.Id,
                        Time = p.Date,//.ToString("HH:mm"),
                        CreatedAt = p.Date//.ToString("g", CultureInfo.CurrentCulture)
                    })
                });
            }

            result.TotalExtraTime = result.Days.Sum(x => x.ExtraTime);
            result.TotalMissingTime = result.Days.Sum(x => x.MissingTime);
            result.TotalWorkedTime = result.Days.Sum(x => x.WorkedTime);

            return result;
        }
    }
}
