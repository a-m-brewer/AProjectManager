using System.Collections.Generic;
using AProjectManager.Git.Models;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositoryRegisterManager
    {
        RepositoryRegister UpdateRegister(string repositoriesFileName);
        RepositoryRegister GetRegister();
        Dictionary<string, bool> RepositoryExistInRegister(IEnumerable<string> repositorySlugs);
        List<RepositoryRemoteLink> GetAvailableRepositories();
        List<RepositoryRemoteLink> GetAvailableRepositories(IEnumerable<string> slugs);
        List<RepositoryRemoteLink> GetAvailableRepositories(string name);
    }
}