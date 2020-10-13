using Clockify.Core.Data;
using Clockify.Tracking.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Clockify.Tracking.Domain.Data
{
    public interface IConfigurationRepository : IRepository<Configuration>
    {
        void CreateConfiguration(Configuration config);
        Task<Configuration> FindByUser(Guid userId);
    }
}
