using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class RepositoryGroup
    {
        public string GroupName { get; set; }
        public List<string> RepositorySlugs { get; set; } = new List<string>();
    }
}