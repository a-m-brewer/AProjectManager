using System.IO;

namespace AProjectManager.Docker.Models
{
    public class DockerComposeArguments
    {
        public bool Build { get; set; }
        public bool Daemon { get; set; } = true;
        public string File { get; set; } = "docker-comopose.yml";
        public string BaseDirectory { get; set; } = "./";
        public string FullPath => Path.Combine(BaseDirectory, File);

        public override string ToString()
        {
            return
                $"{nameof(Build)}: {Build}, {nameof(Daemon)}: {Daemon}, {nameof(File)}: {File}, {nameof(BaseDirectory)}: {BaseDirectory}, {nameof(FullPath)}: {FullPath}";
        }
    }
}