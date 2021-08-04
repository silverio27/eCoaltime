using System.Threading;
using System.Threading.Tasks;

namespace Posts.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}