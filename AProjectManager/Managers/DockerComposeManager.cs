using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Docker;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;

namespace AProjectManager.Managers
{
    public class DockerComposeManager : IDockerComposeManager
    {
        private readonly IRepositoryProvider _repositoryProvider;

        public DockerComposeManager(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public async Task Up(DockerComposeUpRequest request, CancellationToken cancellationToken = default)
        {
            var repositories = _repositoryProvider.GetAvailableRepositories(request.Name);

            foreach (var repository in repositories)
            {
                var args = request.ToDockerCompose(repository.Local.Location);
                var process = DockerComposeProcessBuilder.Up(args);
                Console.WriteLine($"Starting Docker Container: {args}");
                process.Start();
                Console.WriteLine($"Stated Docker Container: {args}");
            }
        }

        public async Task Down(DockerComposeDownRequest request, CancellationToken cancellationToken = default)
        {
            var repositories = _repositoryProvider.GetAvailableRepositories(request.Name);

            foreach (var repository in repositories)
            {
                var args = request.ToDockerCompose(repository.Local.Location);
                var process = DockerComposeProcessBuilder.Down(args);
                Console.WriteLine($"Stopping Docker Container: {args}");
                process.Start();
                Console.WriteLine($"Stopped Docker Container: {args}");
            }
        }

        public async Task Super(DockerComposeSuperRequest request, CancellationToken cancellationToken = default)
        {
            var repositories = _repositoryProvider.GetAvailableRepositories(request.Name);

            foreach (var process in repositories
                .Select(repository => Path.Combine(repository.Local.Location, request.FileName))
                .Select(fullPath => DockerComposeProcessBuilder.Super(fullPath, request.Arguments.ToArray())))
            {
                Console.WriteLine($"running command {string.Join(" ", request.Arguments)}");
                process.Start();
                Console.WriteLine($"ran command {string.Join(" ", request.Arguments)}");
            }
        }
    }
}