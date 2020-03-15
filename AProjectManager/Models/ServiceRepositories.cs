using System.Collections.Generic;
using AProjectManager.Constants;
using AProjectManager.Git.Models;

namespace AProjectManager.Models
{
    public class ServiceRepositories
    {
        public string Name { get; set; }
        public string Service { get; set; }
        public List<RepositoryRemoteLink> Repositories { get; set; } = new List<RepositoryRemoteLink>();
    }
}