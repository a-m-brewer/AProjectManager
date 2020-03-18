using System.Collections.Generic;
using AProjectManager.Cli.Enums;
using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("repository-source", HelpText = "For managing repositories and their sources")]
    public class RepositorySourceVerb
    {
        [Option('a', "action", HelpText = "What action to perform on repositories", Default = RepositoryActions.List)]
        public RepositoryActions Actions { get; set; }
        
        [Option('r', "repositories", HelpText = "Paths of repositories", Separator = ' ')]
        public IEnumerable<string> RepositoryPaths { get; set; }
    }
}