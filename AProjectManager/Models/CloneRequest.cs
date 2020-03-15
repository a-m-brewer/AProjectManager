using AProjectManager.BitBucket.Models;

namespace AProjectManager.Models
{
    public class CloneRequest
    {
        public GetRepositoriesRequest GetRepositoriesRequest { get; set; }
        public string CloneDirectory { get; set; }
    }
}