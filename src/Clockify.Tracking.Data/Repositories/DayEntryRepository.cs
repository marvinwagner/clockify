using Clockify.Core.Data;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockify.Tracking.Data.Repository
{
    public class DayEntryRepository : IDayEntryRepository
    {
        private readonly TrackingContext _context;

        public DayEntryRepository(TrackingContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void CreateDay(DayEntry day)
        {
            _context.DayEntries.Add(day);
        }

        public void CreatePoint(TimeEntry time)
        {
            _context.TimeEntries.Add(time);
        }

        public void RemoveDay(DayEntry day)
        {
            _context.DayEntries.Remove(day);
        }

        public void RemovePoint(TimeEntry time)
        {
            _context.TimeEntries.Remove(time);
        }

        public async Task<DayEntry> FindByDay(Guid userId, DateTime date)
        {
            return await _context.DayEntries
                .Include(x => x.Points)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Date.Date == date.Date);
        }

        public async Task<IEnumerable<DayEntry>> FindByPeriod(Guid userId, DateTime start, DateTime end)
        {
            return await _context.DayEntries
                .Include(x => x.Points)
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Date.Date >= start.Date && x.Date.Date <= end.Date)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        public async Task<TimeEntry> FindTimeEntry(Guid timeId)
        {
            return await _context.TimeEntries.FirstOrDefaultAsync(x => x.Id == timeId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}