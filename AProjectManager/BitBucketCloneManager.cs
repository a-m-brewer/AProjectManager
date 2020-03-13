using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Constants;
using AProjectManager.Domain.BitBucket;
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

        public BitBucketCloneManager(
            IBitBucketClient bitBucketClient,
            IFileConfigManager fileConfigManager)
        {
            _bitBucketClient = bitBucketClient;
            _fileConfigManager = fileConfigManager;
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

            _fileConfigManager.WriteData(repositoriesDto,
                ConfigFiles.RepoConfigName("bitbucket", cloneRequest.GetRepositoriesRequest.User));
        }
    }
}