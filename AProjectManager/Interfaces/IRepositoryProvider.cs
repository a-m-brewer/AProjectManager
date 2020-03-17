using System.Collections.Generic;
using AProjectManager.Git.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositoryProvider
    {
        Dictionary<string, bool> RepositoriesExist(IEnumerable<string> repositorySlugs);
        List<RepositoryRemoteLink> GetAvailableRepositories();
        List<RepositoryRemoteLink> GetAvailableRepositories(IEnumerable<string> slugs);
        List<RepositoryRemoteLink> GetAvailableRepositories(string name);
    }
}