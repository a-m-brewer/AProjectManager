using System;
using AProjectManager.Domain.Git;
using AProjectManager.Git;

namespace AProjectManager.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var clone = Clone.Create("/Users/adambrewer/source/avoid-plugins",
                "http://avoid-network.xyz/a-m-brewer/avoid-plugins.git", "avoid-plugin");
            
            var repositoryManager = new RepositoryManager();

            var process = repositoryManager.Clone(clone);

            process.Start();
        }
    }
}