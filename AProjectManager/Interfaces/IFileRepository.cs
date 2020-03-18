using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IFileRepository
    {
        RepositoryGroup GetGroup(string groupName);
        RepositorySession GetSession(string sessionName);
        RepositorySource GetServiceRepositories(string serviceName, string userName);
        RepositorySource GetServiceRepositories(string fileName);
        RepositoryRegister GetRepositoryRegister();
        RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup);
        RepositorySession WriteSession(RepositorySession repositorySession);
        RepositorySource WriteServiceRepositories(RepositorySource repositorySource);
        RepositoryRegister WriteRepositoryRegister(RepositoryRegister repositoryRegister);
    }
}