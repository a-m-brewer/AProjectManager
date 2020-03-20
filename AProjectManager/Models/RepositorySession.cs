using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class RepositorySession
    {
        public string Name { get; set; }
        public string BranchName { get; set; }
        public List<string> RepositorySlugs { get; set; } = new List<string>();
        public string RepositoryGroupName { get; set; }
        public bool Active { get; set; }
    }
}