using System.Collections.Generic;
using AProjectManager.Cli.Enums;
using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("git", HelpText = "Run git commands on sets of repositories")]
    public class GitVerb
    {
        [Option('a', "action", HelpText = "Action to perform on a set of repositories")]
        public GitAction Action { get; set; }
        [Option('n', "names", HelpText = "Names of the resource to interact with", Separator = ' ')]
        public IEnumerable<string> Names { get; set; }
        [Option('s', "super", HelpText = "Run any docker compose command, put arguments after -, remember to put quotes around the command")]
        public string Super { get; set; }
        [Option('b', "branch", HelpText = "Branch name to checkout", Default = "master")]
        public string Branch { get; set; }
    }
}