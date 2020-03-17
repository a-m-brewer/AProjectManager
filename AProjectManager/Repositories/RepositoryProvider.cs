using System.Collections.Generic;
using System.Linq;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;

namespace AProjectManager.Repositories
{
    // I Know the folder is repositories this class is one, I just could not bring myself to name it RepositoryRepository
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly IRepositoryRegisterManager _registerManager;
        private readonly IFileRepository _fileRepository;

        public RepositoryProvider(
            IRepositoryRegisterManager registerManager,
            IFileRepository fileRepository)
        {
            _registerManager = registerManager;
            _fileRepository = fileRepository;
        }
        
        public Dictionary<string, bool> RepositoriesExist(IEnumerable<string> repositorySlugs)
        {
            var repositories = GetAvailableRepositories()
                .Select(s => s.Slug)
                .ToList();

            return repositorySlugs.ToDictionary(
                repositorySlug => repositorySlug, 
                repositorySlug => repositories.Contains(repositorySlug));
        }

        public List<RepositoryRemoteLink> GetAvailableRepositories()
        {
            var repositoryFiles = _registerManager.GetRegister();
            return repositoryFiles.FileNames.SelectMany(fileName => _fileRepository.GetServiceRepositories(fileName).Repositories).ToList();
        }

        public List<RepositoryRemoteLink> GetAvailableRepositories(IEnumerable<string> slugs)
        {
            return (from repository in GetAvailableRepositories()
                join slug in slugs on repository.Slug equals slug
                select repository).ToList();
        }

        public List<RepositoryRemoteLink> GetAvailableRepositories(string name)
        {
            var repositories = _fileRepository.GetSession(name)?.RepositorySlugs 
                               ?? _fileRepository.GetGroup(name)?.RepositorySlugs 
                               ?? new List<string> {name};

            return GetAvailableRepositories(repositories);
        }
    }
}