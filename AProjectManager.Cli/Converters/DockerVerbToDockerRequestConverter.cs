using AProjectManager.Cli.Verbs;
using AProjectManager.Models;

namespace AProjectManager.Cli.Converters
{
    public static class DockerVerbToDockerRequestConverter
    {
        public static DockerComposeUpRequest ToDockerComposeUpRequest(this DockerComposeVerb verb)
        {
            return new DockerComposeUpRequest
            {
                Build = verb.Build,
                FileName = verb.FileName,
                Name = verb.Name
            };
        }

        public static DockerComposeDownRequest ToDockerComposeDownRequest(this DockerComposeVerb verb)
        {
            return new DockerComposeDownRequest
            {
                Build = verb.Build,
                FileName = verb.FileName,
                Name = verb.Name
            };
        }
    }
}