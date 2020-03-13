using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface ICloneManager
    {
        Task Clone(CloneRequest getRepositoriesRequest, CancellationToken cancellationToken = default);
    }
}