using System;
using System.Collections.Generic;
using AProjectManager.Domain.Git;
using AProjectManager.Git;
using Avoid.Cli;

namespace AProjectManager.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var clone = Clone.Create("~/source/avoid-plugins",
                "http://avoid-network.xyz/a-m-brewer/avoid-plugins.git", "avoid-plugin");

            var process = new List<IProcess>
            {
                RepositoryManager.Clone(clone),
                RepositoryManager.Fetch((LocalRepository)clone.Local)
            };

            var runnable = process.ToRunnableProcess();

            runnable.Start();
        }
    }
}