using System.Collections.Generic;
using System.IO;
using System.Linq;
using AProjectManager.BitBucket.Models;
using AProjectManager.Git.Models;

namespace AProjectManager.Extensions
{
    public static class RepositoryExtensions
    {
        public static RepositoryRemoteLink ToRepository(this ApiRepo repo, string cloneDirectory = "")
        {
            return new RepositoryRemoteLink
            {
                Slug = repo.Slug,
                Local = new LocalRepository
                {
                    Location = string.IsNullOrEmpty(cloneDirectory) ? repo.Slug : Path.Combine(cloneDirectory, repo.Slug),
                    Name = repo.Name
                },
                Origin = new RemoteRepository
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