using AProjectManager.Cli.Models;
using AProjectManager.Models;

namespace AProjectManager.Cli.Converters
{
    public static class SessionVerbToSessionRequest
    {
        public static SessionStartRequest ToSessionStartRequest(this SessionVerb sessionVerb)
        {
            return new SessionStartRequest
            {
                BranchName = sessionVerb.BranchName,
                Checkout = sessionVerb.Checkout,
                RepositoryGroupName = sessionVerb.GroupName,
                Slugs = sessionVerb.Slugs,
                DockerComposeMetaData = new DockerComposeMetaData
                {
                    ComposeFile = sessionVerb.DockerComposeFileName,
                    Run = sessionVerb.DockerCompose
                }
            };
        }

        public static SessionExitRequest ToSessionExitRequest(this SessionVerb sessionVerb)
        {
            return new SessionExitRequest
            {
                BranchName = sessionVerb.BranchName,
                DockerComposeMetaData = new DockerComposeMetaData
                {
                    ComposeFile = sessionVerb.DockerComposeFileName,
                    Run = sessionVerb.DockerCompose
                }
            };
        }

        public static SessionCheckoutRequest ToSessionCheckoutRequest(this SessionVerb sessionVerb)
        {
            return new SessionCheckoutRequest
            {
                BranchName = sessionVerb.BranchName,
                DockerComposeMetaData = new DockerComposeMetaData
                {
                    ComposeFile = sessionVerb.DockerComposeFileName,
                    Run = sessionVerb.DockerCompose
                }
            };
        }
    }
}