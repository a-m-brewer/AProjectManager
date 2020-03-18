using System.Collections.Generic;
using AProjectManager.Git.Models;

namespace AProjectManager.Models
{
    public class RepositorySource
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Source { get; set; }
        public List<RepositoryRemoteLink> Repositories { get; set; } = new List<RepositoryRemoteLink>();
    }
}