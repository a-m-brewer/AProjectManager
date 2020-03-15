using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface ICloneManager
    {
        Task<ServiceRepositories> Clone(CloneRequest getRepositoriesRequest, CancellationToken cancellationToken = default);
    }
}