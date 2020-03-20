using System.Collections.Generic;
using System.Linq;
using AProjectManager.Git;
using AProjectManager.Interfaces;
using AProjectManager.Models;

namespace AProjectManager.Managers
{
    public class PrintManager : IPrintManager
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IFileRepository _fileRepository;

        public PrintManager(
            IRepositoryProvider repositoryProvider,
            IFileRepository fileRepository)
        {
            _repositoryProvider = repositoryProvider;
            _fileRepository = fileRepository;
        }
        
        public List<RepositoryPrintRow> GetRepositoryData()
        {
            var repos = _repositoryProvider.GetAvailableRepositories()
                .Select(s => new RepositoryPrintRow
                {
                    Slug = s.Slug,
                    Branch = s.Local.CurrentBranch(),
                    LastCommitMessage = s.Local.GetLogMessages().LastOrDefault()
                }).ToList();
            
            return repos;
        }

        public List<SessionPrintRow> GetSessionData()
        {
            var sessionData = _fileRepository.GetSessions();

            return sessionData.Select(s => new SessionPrintRow
            {
                Name = s.Name,
                Active = s.Active,
                GroupName = s.RepositoryGroupName,
                Slugs = string.Join(", ", s.RepositorySlugs)
            }).ToList();
        }

        public List<GroupPrintRow> GetGroupData()
        {
            var groupData = _fileRepository.GetGroups();

            return groupData.Select(s => new GroupPrintRow
            {
                Name = s.GroupName,
                Slugs = string.Join(", ", s.RepositorySlugs)
            }).ToList();
        }

        public List<SourcePrintRow> GetRepositorySourcesData()
        {
            var sources = _fileRepository.GetServiceRepositories();
            return sources.Select(s => new SourcePrintRow
            {
                Domain = s.Domain,
                Name = s.Name,
                Source = s.Source
            }).ToList();
        }
    }
}