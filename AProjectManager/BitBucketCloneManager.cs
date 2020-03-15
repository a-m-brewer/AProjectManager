using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Git;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using Avoid.Cli;

namespace AProjectManager
{
    public class BitBucketCloneManager : ICloneManager
    {
        private readonly IBitBucketClient _bitBucketClient;
        private readonly IFileConfigManager _fileConfigManager;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public BitBucketCloneManager(
            IBitBucketClient bitBucketClient,
            IFileConfigManager fileConfigManager,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _bitBucketClient = bitBucketClient;
            _fileConfigManager = fileConfigManager;
            _repositoryRegisterManager = repositoryRegisterManager;
        }

        public async Task Clone(CloneRequest cloneRequest, CancellationToken cancellationToken = default)
        {
            var repositories = await _bitBucketClient.GetRepositoriesAsync(cloneRequest.GetRepositoriesRequest, cancellationToken);

            var repositoriesDto = repositories.ToRepositories(cloneRequest.CloneDirectory);

            foreach (var cloneProcess in repositoriesDto
                .Select(link => RepositoryManager
                    .Clone(Domain.Git.Clone.Create(link.Local.Location, link.Origin.Location, link.Local.Name))
                    .ToRunnableProcess()))
            {
                cloneProcess.Start();
            }

            var repositoryName =
                ConfigFiles.RepoConfigName(Services.BitBucket, cloneRequest.GetRepositoriesRequest.User);
            
            _fileConfigManager.WriteData(repositoriesDto, repositoryName, ConfigPaths.Repositories);

            _repositoryRegisterManager.UpdateRegister(repositoryName);
        }
    }
}