namespace AProjectManager.Models
{
    public class SessionCheckoutRequest
    {
        public string BranchName { get; set; }
        public DockerComposeMetaData DockerComposeMetaData { get; set; }
    }
}