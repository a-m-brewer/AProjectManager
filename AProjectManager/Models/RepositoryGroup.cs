using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class RepositoryGroup
    {
        public string GroupName { get; set; }
        public List<string> RepositoryLocations { get; set; }
    }
}