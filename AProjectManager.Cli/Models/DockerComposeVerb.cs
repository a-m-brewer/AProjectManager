using AProjectManager.Cli.Enums;
using CommandLine;

namespace AProjectManager.Cli.Models
{
    [Verb("docker-compose", HelpText = "Run Docker Compose Actions on Repoitsory/Group/Session")]
    public class DockerComposeVerb
    {
        [Option('b', "build", HelpText = "Build the container locally", Default = true)]
        public bool Build { get; set; }
        [Option('f', "filename", HelpText = "The filename of the docker item", Default = "docker-compose.yml")]
        public string FileName { get; set; }
        [Option('n', "name", HelpText = "Name of the Group/Session or Repository")]
        public string Name { get; set; }
        [Option('a', "action", HelpText = "docker compose action", Default = DockerAction.Up)]
        public DockerAction DockerAction { get; set; }
    }
}