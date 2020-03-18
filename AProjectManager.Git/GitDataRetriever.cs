using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AProjectManager.Git.Models;
using Avoid.Cli;

namespace AProjectManager.Git
{
    public static class GitDataRetriever
    {
        public static List<Branch> GetBranches(this LocalRepository localRepository)
        {
            var branches = new List<Branch>();

            void Callback(object o, DataReceivedEventArgs e)
            {
                if (string.IsNullOrEmpty(e.Data))
                {
                    return;
                }
                
                var noWhiteSpace = Regex.Replace(e.Data, @"\s+", "").Trim(' ', '*');

                if (noWhiteSpace.Contains("->"))
                {
                    return;
                }

                if (noWhiteSpace.StartsWith("remotes/origin"))
                {
                    var branchName = noWhiteSpace.Split("/").Last();
                    branches.Add(new Branch
                    {
                        Name = branchName,
                        FullName = noWhiteSpace,
                        Remote = true
                    });
                }
                else
                {
                    branches.Add(new Branch
                    {
                        Name = noWhiteSpace,
                        FullName = noWhiteSpace,
                        Remote = false
                    });
                }
            }

            RepositoryManager.Branch(localRepository, Callback).ToRunnableProcess().Start();

            return branches;
        }

        public static bool BranchExists(this LocalRepository localRepository, string branchName)
        {
            return GetBranches(localRepository).Select(s => s.Name).Contains(branchName);
        }

        public static string GetRemoteUrl(string localRepositoryPath)
        {
            var remote = string.Empty;

            void Callback(object o, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    remote = e.Data.Replace(" ", "");
                }
            }

            RepositoryManager.Config(localRepositoryPath, Callback).ToRunnableProcess().Start();

            return remote;
        }
    }
}