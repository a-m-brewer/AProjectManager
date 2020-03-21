using System.Collections.Generic;
using System.Threading.Tasks;

namespace AProjectManager.Interfaces
{
    public interface IGitManager
    {
        Task Pull();
        Task Pull(IEnumerable<string> names);
        Task Fetch();
        Task Fetch(IEnumerable<string> names);
        Task Checkout(string branchName);
        Task Checkout(IEnumerable<string> names, string branchName);
        Task Super(IReadOnlyCollection<string> arguments);
        Task Super(IEnumerable<string> names, IReadOnlyCollection<string> arguments);
    }
}