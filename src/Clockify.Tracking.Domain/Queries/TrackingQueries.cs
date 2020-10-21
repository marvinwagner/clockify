using Clockify.Core.Extensions;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Models;
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

        public async Task<PeriodViewModel> LoadRange(Guid userId, DateTime start, DateTime end)
        {
            var days = await _dayEntryRepository.FindByPeriod(userId, start, end);
            var config = await _configurationRepository.FindByUser(userId);

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
                    WorkedTime = day.WorkedTime,
                    Points = day.Points.OrderBy(p => p.Date).Select(p => new TimeEntryViewModel
                    {
                        Id = p.Id,
                        Time = p.Date,//.ToString("HH:mm"),
                        CreatedAt = p.Date//.ToString("g", CultureInfo.CurrentCulture)
                    })
                });
            }
            var workedTime = result.Days.Sum(x => x.WorkedTime);
            var missingTime = TimeSpan.Zero; //result.Days.Sum(x => x.MissingTime);
            var extraTime = TimeSpan.Zero;//result.Days.Sum(x => x.ExtraTime);
            
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
