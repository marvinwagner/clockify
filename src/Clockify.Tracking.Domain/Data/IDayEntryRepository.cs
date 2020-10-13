using Clockify.Core.Data;
using Clockify.Tracking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clockify.Tracking.Domain.Data
{
    public interface IDayEntryRepository : IRepository<DayEntry>
    {
        void CreateDay(DayEntry day);
        void RemoveDay(DayEntry day);
        void CreatePoint(TimeEntry time);
        void RemovePoint(TimeEntry time);
        Task<DayEntry> FindByDay(Guid userId, DateTime date);
        Task<IEnumerable<DayEntry>> FindByPeriod(Guid userId, DateTime start, DateTime end);
        Task<TimeEntry> FindTimeEntry(Guid timeId);
    }
}
