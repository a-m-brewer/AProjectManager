using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Git;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using Avoid.Cli;

namespace AProjectManager.Managers.BitBucket
{
    public class BitBucketCloneManager : ICloneManager
    {
        private readonly IBitBucketClient _bitBucketClient;
        private readonly IFileRepository _fileRepository;

        public BitBucketCloneManager(
            IBitBucketClient bitBucketClient,
            IFileRepository fileRepository)
        {
            _bitBucketClient = bitBucketClient;
            _fileRepository = fileRepository;
        }

        public async Task<ServiceRepositories> Clone(CloneRequest cloneRequest, CancellationToken cancellationToken = default)
        {
            var existingRepository =
                _fileRepository.GetServiceRepositories(Services.BitBucket, cloneRequest.GetRepositoriesRequest.User) ?? new ServiceRepositories
                {
                    Name = cloneRequest.GetRepositoriesRequest.User,
                    Service = Services.BitBucket
                };
            
            var existingSlugs = existingRepository.Repositories.Select(s => s.Slug).ToList();
            
            var repositories = await _bitBucketClient.GetRepositoriesAsync(cloneRequest.GetRepositoriesRequest, cancellationToken);

            var repositoriesToAdd = repositories
                .ToRepositories(cloneRequest.CloneDirectory)
                .Where(rep => !existingSlugs.Contains(rep.Slug))
                .ToList();

            foreach (var cloneProcess in repositoriesToAdd
                .Select(repo => new
                {
                    RunnableProcess = RepositoryManager
                        .Clone(Git.Models.Clone.Create(repo.Local.Location, repo.Origin.Location, repo.Local.Name))
                        .ToRunnableProcess(),
                    Repo = repo
                }))
            {
                Console.WriteLine($"Cloning: {cloneProcess.Repo.Slug} into: {cloneProcess.Repo.Local.Location}");
                cloneProcess.RunnableProcess.Start();
                Console.WriteLine($"Cloned: {cloneProcess.Repo.Slug} into: {cloneProcess.Repo.Local.Location}");
            }

            existingRepository.Repositories.AddRange(repositoriesToAdd);

            return _fileRepository.WriteServiceRepositories(existingRepository);
        }
    }
}