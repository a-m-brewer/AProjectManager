using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager
{
    public class RepositoryGroupManager : IRepositoryGroupManager
    {
        private readonly IFileConfigManager _fileConfigManager;

        public RepositoryGroupManager(IFileConfigManager fileConfigManager)
        {
            _fileConfigManager = fileConfigManager;
        }
        
        public async Task<RepositoryGroup> Add(GroupAddRequest groupAddRequest)
        {
            var existingGroup = _fileConfigManager.GetFromFile<RepositoryGroup>(groupAddRequest.GroupName);

            var updatedItem = existingGroup ?? new RepositoryGroup();

            updatedItem.GroupName = groupAddRequest.GroupName;

            if (updatedItem.RepositoryLocations == null)
            {
                updatedItem.RepositoryLocations = groupAddRequest.ProjectLocations;
            }
            else
            {
                var toAdd = groupAddRequest.ProjectLocations.Where(w => !updatedItem.RepositoryLocations.Contains(w));
                updatedItem.RepositoryLocations.AddRange(toAdd);
            }
            
            return _fileConfigManager.WriteData(updatedItem, groupAddRequest.GroupName);
        }
    }
}