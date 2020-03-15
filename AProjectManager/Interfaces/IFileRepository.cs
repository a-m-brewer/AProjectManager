using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IFileRepository
    {
        RepositoryGroup GetGroup(string groupName);
        RepositorySession GetSession(string sessionName);
        ServiceRepositories GetServiceRepositories(string serviceName, string userName);
        ServiceRepositories GetServiceRepositories(string fileName);
        RepositoryRegister GetRepositoryRegister();
        RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup);
        RepositorySession WriteSession(RepositorySession repositorySession);
        ServiceRepositories WriteServiceRepositories(ServiceRepositories serviceRepositories);
        RepositoryRegister WriteRepositoryRegister(RepositoryRegister repositoryRegister);
    }
}