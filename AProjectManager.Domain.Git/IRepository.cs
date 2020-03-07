namespace AProjectManager.Domain.Git
{
    public interface IRepository
    {
        string Name { get; set; }
        string Location { get; set; }
    }
}