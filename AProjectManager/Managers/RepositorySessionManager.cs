using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.Extensions;
using AProjectManager.Git;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Utils;
using Avoid.Cli;

namespace AProjectManager.Managers
{
    public class RepositorySessionManager : IRepositorySessionManager
    {
        private readonly IFileRepository _fileRepository;
        private readonly IRepositoryRegisterManager _repositoryRegisterManager;

        public RepositorySessionManager(
            IFileRepository fileRepository,
            IRepositoryRegisterManager repositoryRegisterManager)
        {
            _fileRepository = fileRepository;
            _repositoryRegisterManager = repositoryRegisterManager;
        }
        
        public async Task<RepositorySession> Start(SessionStartRequest sessionStartRequest, CancellationToken cancellationToken = default)
        {
            var repositorySession = GetSession(sessionStartRequest.BranchName);
            
            if (sessionStartRequest.RepositoryGroupName != null && !TryImportRepositoryGroup(sessionStartRequest.RepositoryGroupName, repositorySession))
            {
                return repositorySession;
            }

            if (sessionStartRequest.Slugs != null && !TryAddSlugsIfNotExist(sessionStartRequest.Slugs.ToList(), repositorySession))
            {
                return repositorySession;
            }

            _fileRepository.WriteSession(repositorySession);

            if (!sessionStartRequest.Checkout)
            {
                return repositorySession;
            }
            
            CheckoutRepositories(repositorySession.RepositorySlugs, repositorySession.BranchName);

            return repositorySession;
        }

        private RepositorySession GetSession(string branchName)
        {
            return _fileRepository.GetSession(branchName) ?? new RepositorySession
            {
                Name = branchName, 
                BranchName = branchName
            };
        }

        private bool TryImportRepositoryGroup(string repositoryGroupName, RepositorySession repositorySession)
        {
            Console.WriteLine($"Importing Group: {repositoryGroupName} into session");

            var repositoryGroup = _fileRepository.GetGroup(repositoryGroupName);

            if (repositoryGroup == null)
            {
                return !ConsoleEvents.YesNoInput($"Could not find Repository Group: {repositoryGroupName}, Continue? ");
            }

            repositorySession.RepositoryGroupName = repositoryGroupName;

            var slugsToAdd =
                repositoryGroup.RepositorySlugs.Where(slug =>
                    !repositorySession.RepositorySlugs.Contains(slug));
                    
            repositorySession.RepositorySlugs.AddRange(slugsToAdd);

            Console.WriteLine($"Imported Group: {repositoryGroupName} into session");

            return true;
        }

        private bool TryAddSlugsIfNotExist(IReadOnlyCollection<string> slugs, RepositorySession repositorySession)
        {
            var slugsAsString = string.Join(", ", slugs);
            Console.WriteLine($"Importing Slugs: {slugsAsString}");
                
            var toAdd = slugs.Where(w => !repositorySession.RepositorySlugs.Contains(w));
               
            var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryRegisterManager.RepositoryExistInRegister(toAdd).SplitExistingAndNonExisting();

            if (repositoriesThatDoNotExist.Any() && !ConsoleEvents.YesNoInput($"Could not find slugs: {string.Join(", ", repositoriesThatDoNotExist)}. Continue? "))
            {
                return false;
            }
                
            repositorySession.RepositorySlugs.AddRange(repositoriesThatExist);
            Console.WriteLine($"Importing Slugs: {slugsAsString}");

            return true;
        }

        private void CheckoutRepositories(IEnumerable<string> repositorySlugs, string branchName)
        {
            var repos = _repositoryRegisterManager
                .GetAvailableRepositories(repositorySlugs)
                .Select(s => s.Local)
                .ToList();
            
            Console.WriteLine("Checking out available repositories");

            foreach (var runnable in repos.Select(repo => new
            {
                Repo = repo,
                Processes = new List<IProcess>
                {
                    RepositoryManager.Fetch(repo),
                    RepositoryManager.Pull(repo),
                    RepositoryManager.Checkout(repo, new Checkout
                    {
                        Branch = new Branch {Name = branchName},
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
        }
    }
}