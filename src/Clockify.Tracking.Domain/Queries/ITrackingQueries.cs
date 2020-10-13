using Clockify.Tracking.Domain.Queries.ViewModels;
using System;
using System.Threading.Tasks;

namespace Clockify.Tracking.Domain.Queries
{
    public interface ITrackingQueries
    {
        Task<PeriodViewModel> LoadMonth(Guid userId, DateTime start, DateTime end);
        Task<ConfigurationViewModel> LoadConfiguration(Guid userId);
    }
}
