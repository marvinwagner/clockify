using Clockify.Core.Data;
using Clockify.Tracking.Domain.Data;
using Clockify.Tracking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Clockify.Tracking.Data.Repository
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly TrackingContext _context;

        public ConfigurationRepository(TrackingContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void CreateConfiguration(Configuration config)
        {
            _context.Configurations.Add(config);
        }

        public async Task<Configuration> FindByUser(Guid userId)
        {
            return await _context.Configurations.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}