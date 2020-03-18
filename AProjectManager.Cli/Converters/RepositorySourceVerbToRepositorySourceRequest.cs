using System.Linq;
using AProjectManager.Cli.Verbs;
using AProjectManager.Models;

namespace AProjectManager.Cli.Converters
{
    public static class RepositorySourceVerbToRepositorySourceRequest
    {
        public static RepositorySourceAddRequest ToAddRequest(this RepositorySourceVerb verb)
        {
            return new RepositorySourceAddRequest
            {
                RepositoryPaths = verb.RepositoryPaths.ToList()
            };
        }
    }
}