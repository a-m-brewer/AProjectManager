using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Git;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using AProjectManager.Models;

namespace AProjectManager.Managers
{
    public class RepositorySourceManager : IRepositorySourceManager
    {
        private readonly IFileRepository _fileRepository;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public RepositorySourceManager(
            IFileRepository fileRepository,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileRepository = fileRepository;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public Task<List<RepositorySource>> Add(RepositorySourceAddRequest request)
        {
            var remotePaths = (from path in request.RepositoryPaths
                select new
                {
                    path = Path.GetFullPath(path),
                    slug = Path.GetFullPath(path).Split(Path.DirectorySeparatorChar).LastOrDefault(),
                    remote = new Uri(GitDataRetriever.GetRemoteUrl(path))
                }).ToList();

            var remotePathsSplit = (from remotePath in remotePaths
                select new
                {
                    remotePath.path,
                    remotePath.remote,
                    remotePath.slug,
                    remotePath.remote.Host,
                    user = remotePath.remote.Segments[1].Replace("/", " "),
                }).ToList();

            var repositorySources = remotePathsSplit.GroupBy(group => group.Host).Select(repositorySource =>
                new RepositorySource
                {
                    Name = repositorySource.First().user.Replace(" ", ""),
                    Domain = repositorySource.Key,
                    Repositories = repositorySource.Select(repo => new RepositoryRemoteLink
                    {
                        Local = new LocalRepository
                        {
                            Location = Path.GetFullPath(repo.path),
                            Name = repo.slug
                        },
                        Origin = new RemoteRepository
                        {
                            Location = repo.remote.AbsoluteUri,
                            Name = $"origin/{repo.slug}"
                        },
                        Slug = repo.slug
                    }).ToList(),
                    Source = Sources.Local
                }).ToList();

            foreach (var repositorySource in repositorySources)
            {
                _repositoryRegisterManager.UpdateRegister(repositorySource.GetFileName());
                _fileRepository.WriteServiceRepositories(repositorySource);
            }
            
            return Task.FromResult(repositorySources);
        }
    }
}