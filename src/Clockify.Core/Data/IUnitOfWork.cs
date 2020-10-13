using System.Threading.Tasks;

namespace Clockify.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
