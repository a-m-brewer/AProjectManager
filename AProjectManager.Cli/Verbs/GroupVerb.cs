using System.Collections.Generic;
using AProjectManager.Cli.Enums;
using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("group", HelpText = "Manage groups of repositories")]
    public class GroupVerb
    {
        [Option('g', "group-name", HelpText = "The Name of the Set of Repositories")]
        public string GroupName { get; set; }
        
        [Option('s', "slugs", HelpText = "List of repository slugs to be part of group", Separator = ' ')]
        public IEnumerable<string> RepositorySlugs { get; set; }
        
        [Option('a', "action", HelpText = "Action to perform on repository group e.g. add")]
        public RepositoryGroupAction Action { get; set; }
        
    }
}