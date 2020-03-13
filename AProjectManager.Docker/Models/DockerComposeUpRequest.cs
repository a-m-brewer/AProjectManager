using System.IO;

namespace AProjectManager.Docker.Models
{
    public class DockerComposeUpRequest : DockerComposeDownRequest
    {
        public bool Build { get; set; }
        public bool Daemon { get; set; } = true;
    }

    public class DockerComposeDownRequest
    {
        public string File { get; set; } = "docker-comopose.yml";
        public string BaseDirectory { get; set; } = "./";
        public string FullPath => Path.Combine(BaseDirectory, File);
    }
}