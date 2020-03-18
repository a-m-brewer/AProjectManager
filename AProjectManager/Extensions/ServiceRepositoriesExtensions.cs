using AProjectManager.Constants;
using AProjectManager.Models;

namespace AProjectManager.Extensions
{
    public static class ServiceRepositoriesExtensions
    {
        public static string GetFileName(this RepositorySource repositorySource)
        {
            return ConfigFiles.RepoConfigName(repositorySource.Domain, repositorySource.Name);
        }
    }
}