using System.Threading.Tasks;

namespace AProjectManager.Cli.Interfaces
{
    public interface ICliService<in T> where T : class
    {
        Task Run(T verb);
    }
}