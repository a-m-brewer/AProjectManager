using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager
{
    public interface IRepositorySessionManager
    {
        Task<RepositorySession> Start(SessionStartRequest sessionStartRequest, CancellationToken cancellationToken = default);
    }
}