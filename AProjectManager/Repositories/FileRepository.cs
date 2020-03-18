using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IFileConfigManager _fileConfigManager;

        public FileRepository(
            IFileConfigManager fileConfigManager)
        {
            _fileConfigManager = fileConfigManager;
        }
        
        public RepositoryGroup GetGroup(string groupName)
        {
            return _fileConfigManager.GetFromFile<RepositoryGroup>(groupName, ConfigPaths.RepositoryGroups);
        }

        public RepositorySession GetSession(string sessionName)
        {
            return _fileConfigManager.GetFromFile<RepositorySession>(sessionName, ConfigPaths.RepositorySessions);
        }

        public RepositorySource GetServiceRepositories(string serviceName, string userName)
        {
            return GetServiceRepositories(ConfigFiles.RepoConfigName(serviceName, userName));
        }
        
        public RepositorySource GetServiceRepositories(string fileName)
        {
            return _fileConfigManager.GetFromFile<RepositorySource>(fileName, ConfigPaths.Repositories);
        }

        public RepositoryRegister GetRepositoryRegister()
        {
            return _fileConfigManager.GetFromFile<RepositoryRegister>(ConfigFiles.RepositoryRegister);
        }
        
        public RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup)
        {
            return _fileConfigManager.WriteData(repositoryGroup, repositoryGroup.GroupName, ConfigPaths.RepositoryGroups);
        }

        public RepositorySession WriteSession(RepositorySession repositorySession)
        {
            return _fileConfigManager.WriteData(repositorySession, repositorySession.Name, ConfigPaths.RepositorySessions);
        }

        public RepositorySource WriteServiceRepositories(RepositorySource repositorySource)
        {
            var file =  _fileConfigManager.WriteData(repositorySource, repositorySource.GetFileName(), ConfigPaths.Repositories);
            return file;
        }

        public RepositoryRegister WriteRepositoryRegister(RepositoryRegister repositoryRegister)
        {
            return _fileConfigManager.WriteData(repositoryRegister, ConfigFiles.RepositoryRegister);
        }
    }
}