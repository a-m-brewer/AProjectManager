using System.Collections.Generic;
using System.Linq;
using AProjectManager.Constants;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager.Managers
{
    public class RepositoryRegisterManager : IRepositoryRegisterManager
    {
        private readonly IFileRepository _fileRepository;

        public RepositoryRegisterManager(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        
        public RepositoryRegister UpdateRegister(string repositoriesFileName)
        {
            var repositoryRegister = GetRegister();

            if (repositoryRegister.FileNames.Contains(repositoriesFileName))
            {
                return repositoryRegister;
            }
            
            repositoryRegister.FileNames.Add(repositoriesFileName);
            return _fileRepository.WriteRepositoryRegister(repositoryRegister);

        }

        public RepositoryRegister GetRegister()
        {
            return _fileRepository.GetRepositoryRegister() ?? new RepositoryRegister();
        }

        public Dictionary<string, bool> RepositoryExistInRegister(IEnumerable<string> repositorySlugs)
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
            var repositoryFiles = GetRegister();
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