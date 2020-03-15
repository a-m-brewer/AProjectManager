namespace AProjectManager.Git.Models
{
    public class Repository : IRepository
    {
        public string Name { get; set; }
        public virtual string Location { get; set; }
    }
}