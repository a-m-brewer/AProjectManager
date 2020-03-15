using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Git;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using AProjectManager.Utils;
using Avoid.Cli;

namespace AProjectManager.Managers
{
    public class RepositorySessionManager : IRepositorySessionManager
    {
        private readonly IFileConfigManager _fileConfigManager;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public RepositorySessionManager(
            IFileConfigManager fileConfigManager,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileConfigManager = fileConfigManager;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public async Task<RepositorySession> Start(SessionStartRequest sessionStartRequest, CancellationToken cancellationToken = default)
        {
            var repositorySession = new RepositorySession { Name = sessionStartRequest.BranchName, BranchName = sessionStartRequest.BranchName, RepositorySlugs = new List<string>()};

            if (sessionStartRequest.RepositoryGroupName != null)
            {
                Console.WriteLine($"Importing Group: {sessionStartRequest.RepositoryGroupName} into session");
                
                var repositoryGroup = _fileConfigManager.GetFromFile<RepositoryGroup>(sessionStartRequest.RepositoryGroupName, ConfigPaths.RepositoryGroups);

                if (repositoryGroup == null)
                {
                    if (!ConsoleEvents.YesNoInput($"Could not find Repository Group: {sessionStartRequest.RepositoryGroupName}, Continue? "))
                    {
                        return repositorySession;
                    }
                }
                else
                {
                    repositorySession.RepositoryGroupName = sessionStartRequest.RepositoryGroupName;
                    repositorySession.RepositorySlugs.AddRange(repositoryGroup.RepositorySlugs);
                }
                
                Console.WriteLine($"Imported Group: {sessionStartRequest.RepositoryGroupName} into session");
            }

            if (sessionStartRequest.Slugs != null)
            {
                var slugsAsString = string.Join(", ", sessionStartRequest.Slugs);
                Console.WriteLine($"Importing Slugs: {slugsAsString}");
                
                var toAdd = sessionStartRequest.Slugs.Where(w => !repositorySession.RepositorySlugs.Contains(w));
               
                var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryRegisterManager.RepositoryExistInRegister(toAdd).SplitExistingAndNonExisting();

                if (repositoriesThatDoNotExist.Any() && !ConsoleEvents.YesNoInput($"Could not find slugs: {string.Join(", ", repositoriesThatDoNotExist)}. Continue? "))
                {
                    return repositorySession;
                }
                
                repositorySession.RepositorySlugs.AddRange(repositoriesThatExist);
                Console.WriteLine($"Importing Slugs: {slugsAsString}");
            }

            _fileConfigManager.WriteData(repositorySession, repositorySession.Name, ConfigPaths.RepositorySessions);

            if (!sessionStartRequest.Checkout)
            {
                return repositorySession;
            }
            
            var repos = _repositoryRegisterManager
                .GetAvailableRepositories(repositorySession.RepositorySlugs)
                .Select(s => s.Local)
                .ToList();
            
            Console.WriteLine($"Checking out available repositories");

            foreach (var runnable in repos.Select(repo => new
            {
                Repo = repo,
                Processes = new List<IProcess>
                {
                    RepositoryManager.Fetch(repo),
                    RepositoryManager.Pull(repo),
                    RepositoryManager.Checkout(repo, new Checkout
                    {
                        Branch = new Branch {Name = sessionStartRequest.BranchName},
                        Create = true
                    })
                }
            }).Select(repo => new
            {
                repo.Repo,
                RunnableProcess = repo.Processes.ToRunnableProcess()
            }))
            {
                Console.WriteLine($"Checking out: {runnable.Repo.Name}");
                runnable.RunnableProcess.Start();
                Console.WriteLine($"Checked out: {runnable.Repo.Name}");
            }
            
            Console.WriteLine($"Checked out available repositories");

            return repositorySession;
        }
    }
}