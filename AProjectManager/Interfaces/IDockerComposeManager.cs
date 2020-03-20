using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IDockerComposeManager
    {
        Task Up(DockerComposeUpRequest request, CancellationToken cancellationToken = default);
        Task Down(DockerComposeDownRequest downRequest, CancellationToken cancellationToken = default);
        Task Super(DockerComposeSuperRequest request, CancellationToken cancellationToken = default);
    }
}