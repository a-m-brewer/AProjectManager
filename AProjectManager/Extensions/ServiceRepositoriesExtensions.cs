using AProjectManager.Constants;
using AProjectManager.Models;

namespace AProjectManager.Extensions
{
    public static class ServiceRepositoriesExtensions
    {
        public static string GetFileName(this ServiceRepositories serviceRepositories)
        {
            return ConfigFiles.RepoConfigName(serviceRepositories.Service, serviceRepositories.Name);
        }
    }
}