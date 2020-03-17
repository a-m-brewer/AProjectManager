using System.Collections.Generic;
using System.Linq;
using AProjectManager.Constants;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;

namespace AProjectManager.Managers
{
    public class RepositoryRegisterManager : IRepositoryRegisterManager
    {
        private readonly IFileRepository _fileRepository;

        public RepositoryRegisterManager(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        
        public RepositoryRegister UpdateRegister(string repositoriesFileName)
        {
            var repositoryRegister = GetRegister();

            if (repositoryRegister.FileNames.Contains(repositoriesFileName))
            {
                return repositoryRegister;
            }
            
            repositoryRegister.FileNames.Add(repositoriesFileName);
            return _fileRepository.WriteRepositoryRegister(repositoryRegister);

        }

        public RepositoryRegister GetRegister()
        {
            return _fileRepository.GetRepositoryRegister() ?? new RepositoryRegister();
        }
    }
}