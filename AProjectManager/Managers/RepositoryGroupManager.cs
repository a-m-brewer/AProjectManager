using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager.Managers
{
    public class RepositoryGroupManager : IRepositoryGroupManager
    {
        private readonly IFileRepository _fileRepository;
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IContinueEvent _continueEvent;

        public RepositoryGroupManager(
            IFileRepository fileRepository,
            IRepositoryProvider repositoryProvider,
            IContinueEvent continueEvent)
        {
            _fileRepository = fileRepository;
            _repositoryProvider = repositoryProvider;
            _continueEvent = continueEvent;
        }
        
        public async Task<RepositoryGroup> Add(GroupAddRequest groupAddRequest)
        {
            var existingGroup = _fileRepository.GetGroup(groupAddRequest.GroupName) ?? new RepositoryGroup
            {
                GroupName = groupAddRequest.GroupName
            };

            var toAdd = groupAddRequest.RepositorySlugs.Where(w => !existingGroup.RepositorySlugs.Contains(w));

            var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryProvider.RepositoriesExist(toAdd).SplitExistingAndNonExisting();

            if (repositoriesThatDoNotExist.Any() && !_continueEvent.Continue($"Could not find slugs: {string.Join(", ", repositoriesThatDoNotExist)}. Continue? "))
            {
                return existingGroup;
            }
            
            existingGroup.RepositorySlugs.AddRange(repositoriesThatExist);

            return _fileRepository.WriteGroup(existingGroup);
        }
    }
}