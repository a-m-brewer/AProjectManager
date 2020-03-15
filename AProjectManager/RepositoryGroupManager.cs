using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using AProjectManager.Utils;

namespace AProjectManager
{
    public class RepositoryGroupManager : IRepositoryGroupManager
    {
        private readonly IFileConfigManager _fileConfigManager;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public RepositoryGroupManager(
            IFileConfigManager fileConfigManager,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileConfigManager = fileConfigManager;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public async Task<RepositoryGroup> Add(GroupAddRequest groupAddRequest)
        {
            var existingGroup = _fileConfigManager.GetFromFile<RepositoryGroup>(groupAddRequest.GroupName, ConfigPaths.RepositoryGroups);

            var updatedItem = existingGroup ?? new RepositoryGroup();

            updatedItem.GroupName = groupAddRequest.GroupName;
            
            updatedItem.RepositorySlugs ??= new List<string>();
            
            var toAdd = groupAddRequest.RepositorySlugs.Where(w => !updatedItem.RepositorySlugs.Contains(w));

            var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryRegisterManager.RepositoryExistInRegister(toAdd).SplitExistingAndNonExisting();

            if (repositoriesThatDoNotExist.Any() && !ConsoleEvents.YesNoInput($"Could not find slugs: {string.Join(", ", repositoriesThatDoNotExist)}. Continue? "))
            {
                return updatedItem;
            }
            
            updatedItem.RepositorySlugs.AddRange(repositoriesThatExist);
            
            return _fileConfigManager.WriteData(updatedItem, groupAddRequest.GroupName, ConfigPaths.RepositoryGroups);
        }
    }
}