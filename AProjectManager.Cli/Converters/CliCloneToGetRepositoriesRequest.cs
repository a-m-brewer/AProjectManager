using AProjectManager.Cli.Models;
using AProjectManager.Domain.BitBucket;

namespace AProjectManager.Cli.Converters
{
    public static class CliCloneToGetRepositoriesRequest
    {
        public static GetRepositoriesRequest ToGetRepositoriesRequest(this CloneVerb verb, string accessToken)
        {
            var request = new GetRepositoriesRequest(verb.User, accessToken);

            if (verb.Role != null)
            {
                request.Role = verb.Role;
            }

            if (verb.ProjectKey != null)
            {
                request.ProjectKey = verb.ProjectKey;
            }

            return request;
        }
    }
}