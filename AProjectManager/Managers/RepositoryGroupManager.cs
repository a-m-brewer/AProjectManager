using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using AProjectManager.Utils;

namespace AProjectManager.Managers
{
    public class RepositoryGroupManager : IRepositoryGroupManager
    {
        private readonly IFileRepository _fileRepository;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public RepositoryGroupManager(
            IFileRepository fileRepository,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileRepository = fileRepository;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public async Task<RepositoryGroup> Add(GroupAddRequest groupAddRequest)
        {
            var existingGroup = _fileRepository.GetGroup(groupAddRequest.GroupName) ?? new RepositoryGroup
            {
                GroupName = groupAddRequest.GroupName
            };

            var toAdd = groupAddRequest.RepositorySlugs.Where(w => !existingGroup.RepositorySlugs.Contains(w));

            var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryRegisterManager.RepositoryExistInRegister(toAdd).SplitExistingAndNonExisting();

            if (repositoriesThatDoNotExist.Any() && !ConsoleEvents.YesNoInput($"Could not find slugs: {string.Join(", ", repositoriesThatDoNotExist)}. Continue? "))
            {
                return existingGroup;
            }
            
            existingGroup.RepositorySlugs.AddRange(repositoriesThatExist);

            return _fileRepository.WriteGroup(existingGroup);
        }
    }
}