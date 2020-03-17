using System;
using System.IO;
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
    }
}