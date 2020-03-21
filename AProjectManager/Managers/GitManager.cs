using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Git;
using AProjectManager.Git.Models;
using AProjectManager.Interfaces;
using Avoid.Cli;

namespace AProjectManager.Managers
{
    public class GitManager : IGitManager
    {
        private readonly IRepositoryProvider _repositoryProvider;

        public GitManager(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }
        
        public async Task Pull()
        {
            var repos = _repositoryProvider.GetAvailableRepositories();
            Pull(repos);
        }
        
        public async Task Pull(IEnumerable<string> names)
        {
            var repos = GetRepositories(names);
            Pull(repos);
        }

        public async Task Fetch()
        {
            var repos = _repositoryProvider.GetAvailableRepositories();
            Fetch(repos);
        }

        public async Task Fetch(IEnumerable<string> names)
        {
            var repos = GetRepositories(names);
            Fetch(repos);
        }

        public async Task Checkout(string branchName)
        {
            var repos = _repositoryProvider.GetAvailableRepositories();
            Checkout(repos, branchName);
        }

        public async Task Checkout(IEnumerable<string> names, string branchName)
        {
            var repos = GetRepositories(names);
            Checkout(repos, branchName);
        }

        public async Task Super(IReadOnlyCollection<string> arguments)
        {
            var repos = _repositoryProvider.GetAvailableRepositories();
            Super(repos, arguments);
        }

        public async Task Super(IEnumerable<string> names, IReadOnlyCollection<string> arguments)
        {
            var repos = GetRepositories(names);
            Super(repos, arguments);
        }

        private static void Pull(IEnumerable<RepositoryRemoteLink> repos)
        {
            foreach (var repo in repos)
            {
                Console.WriteLine("");
                Console.WriteLine($"Pulling updates for {repo.Slug}");
                RepositoryManager.Pull(repo.Local).ToRunnableProcess().Start();
                Console.WriteLine($"Pulled updates for {repo.Slug}");
            }
        }

        private static void Fetch(IEnumerable<RepositoryRemoteLink> repos)
        {
            foreach (var repo in repos)
            {
                Console.WriteLine("");
                Console.WriteLine($"Fetching updates for {repo.Slug}");
                RepositoryManager.Fetch(repo.Local).ToRunnableProcess().Start();
                Console.WriteLine($"Fetched updates for {repo.Slug}");
            }
        }

        private static void Checkout(IEnumerable<RepositoryRemoteLink> repos, string branchName)
        {
            foreach (var repo in repos)
            {
                Console.WriteLine("");
                Console.WriteLine($"Checking out: {repo.Slug} with branch name: {branchName}");
                RepositoryManager.Checkout(repo.Local, new Checkout
                {
                    Branch = new Branch {Name = branchName},
                    Create = !repo.Local.BranchExists(branchName)
                }).ToRunnableProcess().Start();
                Console.WriteLine($"Checked out: {repo.Slug} with branch name: {branchName}");
            }
        }

        private static void Super(IEnumerable<RepositoryRemoteLink> repos, IReadOnlyCollection<string> arguments)
        {
            foreach (var repo in repos)
            {
                Console.WriteLine("");
                Console.WriteLine($"Running Super Command on: {repo.Slug}");
                repo.Local.Super(arguments);
                Console.WriteLine($"Ran Super Command on: {repo.Slug}");
            }
        }

        private IEnumerable<RepositoryRemoteLink> GetRepositories(IEnumerable<string> repoNames)
        {
            return repoNames.SelectMany(repo => _repositoryProvider.GetAvailableRepositories(repo)).ToList();
        }
     }
}