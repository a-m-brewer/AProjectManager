namespace AProjectManager.Git.Models
{
    public interface IRepository
    {
        string Name { get; set; }
        string Location { get; set; }
    }
}