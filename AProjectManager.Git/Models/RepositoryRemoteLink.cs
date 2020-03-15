namespace AProjectManager.Git.Models
{
    public class RepositoryRemoteLink
    {
        public string Slug { get; set; }
        public LocalRepository Local { get; set; }
        public RemoteRepository Origin { get; set; }
    }
}