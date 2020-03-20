using System.Collections.Generic;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IFileRepository
    {
        RepositoryGroup GetGroup(string groupName);
        List<RepositoryGroup> GetGroups();
        RepositorySession GetSession(string sessionName);
        List<RepositorySession> GetSessions();
        RepositorySource GetServiceRepositories(string serviceName, string userName);
        RepositorySource GetServiceRepositories(string fileName);
        List<RepositorySource> GetServiceRepositories();
        RepositoryRegister GetRepositoryRegister();
        RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup);
        RepositorySession WriteSession(RepositorySession repositorySession);
        RepositorySource WriteServiceRepositories(RepositorySource repositorySource);
        RepositoryRegister WriteRepositoryRegister(RepositoryRegister repositoryRegister);
    }
}