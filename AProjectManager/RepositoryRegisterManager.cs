using System;
using System.Collections.Generic;
using System.Linq;
using AProjectManager.Constants;
using AProjectManager.Domain.Git;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager
{
    public class RepositoryRegisterManager : IRepositoryRegisterManager
    {
        private readonly IFileConfigManager _fileConfigManager;

        public RepositoryRegisterManager(IFileConfigManager fileConfigManager)
        {
            _fileConfigManager = fileConfigManager;
        }
        
        public RepositoryRegister UpdateRegister(string repositoriesFileName)
        {
            var repositoryRegister = GetRegister();
            repositoryRegister.FileNames.Add(repositoriesFileName);
            return _fileConfigManager.WriteData(repositoryRegister, ConfigFiles.RepositoryFiles);
        }

        public RepositoryRegister GetRegister()
        {
            return _fileConfigManager.GetFromFile<RepositoryRegister>(ConfigFiles.RepositoryFiles) ?? new RepositoryRegister();
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
            
            return repositoryFiles.FileNames.SelectMany(fileName =>
                    _fileConfigManager.GetFromFile<List<RepositoryRemoteLink>>(fileName, ConfigPaths.Repositories))
                .ToList();
        }

        public List<RepositoryRemoteLink> GetAvailableRepositories(IEnumerable<string> slugs)
        {
            return (from repository in GetAvailableRepositories()
                join slug in slugs on repository.Slug equals slug
                select repository).ToList();
        }
    }
}