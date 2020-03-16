using System.Collections.Generic;

namespace AProjectManager.Models
{
    public class SessionStartRequest
    {
        public string RepositoryGroupName { get; set; }

        public string BranchName { get; set; }
        
        public IEnumerable<string> Slugs { get; set; }
        
        public bool Checkout { get; set; }
        public DockerComposeMetaData DockerComposeMetaData { get; set; }
    }
}