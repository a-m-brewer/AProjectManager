using System.Collections.Generic;
using System.IO;
using System.Linq;
using AProjectManager.Domain.BitBucket;
using AProjectManager.Domain.Git;
using Repository = AProjectManager.Models.Repository;

namespace AProjectManager.Extensions
{
    public static class RepositoryExtensions
    {
        public static RepositoryRemoteLink ToRepository(this ApiRepo repo, string cloneDirectory = "")
        {
            return new RepositoryRemoteLink
            {
                Local = new Domain.Git.Repository
                {
                    Location = string.IsNullOrEmpty(cloneDirectory) ? repo.Slug : Path.Combine(cloneDirectory, repo.Slug),
                    Name = repo.Name
                },
                Origin = new Domain.Git.Repository
                {
                    Location = repo.Links.Clone.First(f => f.Name == "https").Href,
                    Name = $"origin/{repo.Slug}"
                }
            };
        }

        public static List<RepositoryRemoteLink> ToRepositories(this IEnumerable<ApiRepo> apiRepos, string cloneDirectory = "")
        {
            return apiRepos.Select(repo => ToRepository(repo, cloneDirectory)).ToList();
        }
    }
}