using AProjectManager.Domain.BitBucket;

namespace AProjectManager.Models
{
    public class CloneRequest
    {
        public GetRepositoriesRequest GetRepositoriesRequest { get; set; }
        public string CloneDirectory { get; set; }
    }
}