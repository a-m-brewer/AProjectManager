namespace AProjectManager.Domain.Git
{
    public class RepositoryRemoteLink
    {
        public Repository Local { get; set; }
        public Repository Origin { get; set; }
    }
}