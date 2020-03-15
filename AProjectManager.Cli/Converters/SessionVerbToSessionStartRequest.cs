using AProjectManager.Cli.Models;
using AProjectManager.Models;

namespace AProjectManager.Cli.Converters
{
    public static class SessionVerbToSessionStartRequest
    {
        public static SessionStartRequest ToSessionStartRequest(this SessionVerb sessionVerb)
        {
            return new SessionStartRequest
            {
                BranchName = sessionVerb.BranchName,
                Checkout = sessionVerb.Checkout,
                RepositoryGroupName = sessionVerb.ProjectName,
                Slugs = sessionVerb.Slugs
            };
        }
    }
}