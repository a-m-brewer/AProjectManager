namespace AProjectManager.Models
{
    public class SessionExitRequest
    {
        public string BranchName { get; set; }
        public DockerComposeMetaData DockerComposeMetaData { get; set; }
    }
}