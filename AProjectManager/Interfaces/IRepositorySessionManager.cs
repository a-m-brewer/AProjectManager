using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositorySessionManager
    {
        Task<RepositorySession> Start(SessionStartRequest request, CancellationToken cancellationToken = default);

        Task<RepositorySession> Exit(SessionExitRequest request,
            CancellationToken cancellationToken = default);

        Task<RepositorySession> Checkout(SessionCheckoutRequest request,
            CancellationToken cancellationToken = default);
    }
}