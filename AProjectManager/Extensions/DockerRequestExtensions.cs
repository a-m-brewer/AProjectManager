using AProjectManager.Docker.Models;
using AProjectManager.Models;

namespace AProjectManager.Extensions
{
    public static class DockerRequestExtensions
    {
        public static DockerComposeArguments ToDockerCompose(this DockerComposeUpRequest dockerComposeUpRequest, string filePath)
        {
            return new DockerComposeArguments
            {
                Build = dockerComposeUpRequest.Build,
                BaseDirectory = filePath,
                Daemon = true,
                File = dockerComposeUpRequest.FileName
            };
        }
        
        public static DockerComposeArguments ToDockerCompose(this DockerComposeDownRequest dockerComposeUpRequest, string filePath)
        {
            return new DockerComposeArguments
            {
                Build = dockerComposeUpRequest.Build,
                BaseDirectory = filePath,
                Daemon = true,
                File = dockerComposeUpRequest.FileName
            };
        }
    }
}