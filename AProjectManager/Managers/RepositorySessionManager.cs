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
        private readonly IDockerComposeManager _dockerComposeManager;
        private readonly IRepositoryProvider _repositoryProvider;

        public RepositorySessionManager(
            IFileRepository fileRepository,
            IDockerComposeManager dockerComposeManager,
            IRepositoryProvider repositoryProvider)
        {
            _fileRepository = fileRepository;
            _dockerComposeManager = dockerComposeManager;
            _repositoryProvider = repositoryProvider;
        }
        
        public async Task<RepositorySession> Start(SessionStartRequest request, CancellationToken cancellationToken = default)
        {
            var repositorySession = GetSession(request.BranchName);
            
            if (request.RepositoryGroupName != null && !TryImportRepositoryGroup(request.RepositoryGroupName, repositorySession))
            {
                return repositorySession;
            }

            if (request.Slugs != null && !TryAddSlugsIfNotExist(request.Slugs.ToList(), repositorySession))
            {
                return repositorySession;
            }

            repositorySession.Active = true;

            _fileRepository.WriteSession(repositorySession);

            if (request.Checkout)
            {
                CheckoutRepositories(repositorySession.RepositorySlugs, repositorySession.BranchName);
            }

            if (request.DockerComposeMetaData.Run)
            {
                await _dockerComposeManager.Up(new DockerComposeUpRequest
                {
                    Build = true,
                    FileName = request.DockerComposeMetaData.ComposeFile,
                    Name = request.BranchName
                }, cancellationToken);
            }

            return repositorySession;
        }

        public async Task<RepositorySession> Exit(SessionExitRequest request, CancellationToken cancellationToken = default)
        {
            var checkout = await GetSessionAndCheckout(request.BranchName, "master", cancellationToken);
            
            if (request.DockerComposeMetaData.Run)
            {
                await _dockerComposeManager.Down(new DockerComposeDownRequest
                {
                    FileName = request.DockerComposeMetaData.ComposeFile,
                    Name = request.BranchName
                }, cancellationToken);
            }
            
            return checkout;
        }

        public async Task<RepositorySession> Checkout(SessionCheckoutRequest request, CancellationToken cancellationToken = default)
        {
            var checkout = await GetSessionAndCheckout(request.BranchName, request.BranchName, cancellationToken);
            
            if (request.DockerComposeMetaData.Run)
            {
                await _dockerComposeManager.Up(new DockerComposeUpRequest
                {
                    Build = true,
                    FileName = request.DockerComposeMetaData.ComposeFile,
                    Name = request.BranchName
                }, cancellationToken);
            }

            return checkout;
        }

        private async Task<RepositorySession> GetSessionAndCheckout(string sessionName, string branchName, CancellationToken cancellationToken = default)
        {
            var session = GetSession(sessionName);

            session.Active = branchName != "master";

            if (!string.IsNullOrEmpty(session.RepositoryGroupName))
            {
                var group = _fileRepository.GetGroup(session.RepositoryGroupName);
                var extraSlugs = group.RepositorySlugs.Where(slug => !session.RepositorySlugs.Contains(slug));
                session.RepositorySlugs.AddRange(extraSlugs);
            }

            CheckoutRepositories(session.RepositorySlugs, branchName);

            return _fileRepository.WriteSession(session);
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
               
            var (repositoriesThatExist, repositoriesThatDoNotExist) = _repositoryProvider.RepositoriesExist(toAdd).SplitExistingAndNonExisting();

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
            var repos = _repositoryProvider
                .GetAvailableRepositories(repositorySlugs)
                .Select(s => s.Local)
                .ToList();
            
            Console.WriteLine("Checking out available repositories");

            repos.First().GetBranches();
            
            foreach (var runnable in repos.Select(repo =>
            {
                var createBranch = !repo.BranchExists(branchName);

                var processes = new List<IProcess>();

                if (createBranch)
                {
                    processes.AddRange(new List<IProcess>
                    {
                        repo.Fetch(),
                        repo.Pull()
                    });
                }

                processes.Add(repo.Checkout(new Checkout
                {
                    Branch = new Branch {Name = branchName},
                    Create = createBranch
                }));
                
                return new
                {
                    Repo = repo,
                    Processes = processes
                };
                
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