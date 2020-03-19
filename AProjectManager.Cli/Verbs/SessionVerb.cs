using System.Collections.Generic;
using AProjectManager.Cli.Enums;
using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("session", HelpText = "Start a working session with a set of repoisitories")]
    public class SessionVerb
    {
        [Option('g', "group", HelpText = "Start a session from an existing Repository Group")]
        public string GroupName { get; set; }

        [Option('b', "branch", HelpText = "The name of the branch for this session")]
        public string BranchName { get; set; }
        
        [Option('s', "slugs", HelpText = "Other repositories to include during the session", Separator = ' ')]
        public IEnumerable<string> Slugs { get; set; }

        [Option('c', "checkout", HelpText = "Checkout all included repositories on creation of session", Default = true)]
        public bool Checkout { get; set; }

        [Option('a', "action", HelpText = "Action to take with this session e.g. Start")]
        public RepositorySessionAction Action { get; set; }
        
        [Option('d', "docker-compose", HelpText = "Run Docker Compose Action on Entrance/Exit of session", Default = false)]
        public bool DockerCompose { get; set; }
        
        [Option('f', "docker-compose-file", HelpText = "Name of the Docker Compose File", Default = "docker-compose.yml")]
        public string DockerComposeFileName { get; set; }
    }
}