using AProjectManager.Constants;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager.Utils
{
    public class FileRepository : IFileRepository
    {
        private readonly IFileConfigManager _fileConfigManager;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public FileRepository(
            IFileConfigManager fileConfigManager,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileConfigManager = fileConfigManager;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public RepositoryGroup GetGroup(string groupName)
        {
            return _fileConfigManager.GetFromFile<RepositoryGroup>(groupName, ConfigPaths.RepositoryGroups);
        }

        public RepositorySession GetSession(string sessionName)
        {
            return _fileConfigManager.GetFromFile<RepositorySession>(sessionName, ConfigPaths.RepositorySessions);
        }

        public ServiceRepositories GetServiceRepositories(string serviceName, string userName)
        {
            return _fileConfigManager.GetFromFile<ServiceRepositories>(ConfigFiles.RepoConfigName(serviceName, userName),
                ConfigPaths.Repositories);
        }
        
        public RepositoryGroup WriteGroup(RepositoryGroup repositoryGroup)
        {
            return _fileConfigManager.GetFromFile<RepositoryGroup>(repositoryGroup.GroupName, ConfigPaths.RepositoryGroups);
        }

        public RepositorySession WriteSession(RepositorySession repositorySession)
        {
            return _fileConfigManager.WriteData(repositorySession, repositorySession.Name, ConfigPaths.RepositorySessions);
        }

        public ServiceRepositories WriteServiceRepositories(ServiceRepositories serviceRepositories)
        {
            var fileName = ConfigFiles.RepoConfigName(serviceRepositories.Service, serviceRepositories.Name);
            var file =  _fileConfigManager.WriteData(serviceRepositories, fileName, ConfigPaths.Repositories);
            _repositoryRegisterManager.UpdateRegister(fileName);
            return file;
        }
    }
}