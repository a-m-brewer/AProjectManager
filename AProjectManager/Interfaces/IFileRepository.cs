using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IFileRepository
    {
        RepositoryGroup GetGroup(string groupName);
        RepositorySession GetSession(string sessionName);
        ServiceRepositories GetServiceRepositories(string serviceName, string userName);
        RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup);
        RepositorySession WriteSession(RepositorySession repositorySession);
        ServiceRepositories WriteServiceRepositories(ServiceRepositories serviceRepositories);
    }
}