using System.Collections.Generic;
using System.Linq;
using AProjectManager.Git;
using AProjectManager.Interfaces;

namespace AProjectManager.Managers
{
    public class PrintManager : IPrintManager
    {
        private readonly IRepositoryProvider _repositoryProvider;

        public PrintManager(
            IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
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
    }

    public class RepositoryPrintRow
    {
        public string Slug { get; set; }
        public string Branch { get; set; }
        public string LastCommitMessage { get; set; }
    }
}