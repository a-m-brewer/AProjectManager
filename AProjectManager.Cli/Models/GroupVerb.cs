using System.Collections.Generic;
using AProjectManager.Enums;
using CommandLine;

namespace AProjectManager.Cli.Models
{
    [Verb("group", HelpText = "Manage groups of repositories")]
    public class GroupVerb
    {
        [Option('g', "group-name", HelpText = "The Name of the Set of Repositories")]
        public string GroupName { get; set; }
        
        [Option('p', "projects", HelpText = "List of repository slugs to be part of group", Separator = ':')]
        public IEnumerable<string> ProjectSlugs { get; set; }
        
        [Option('a', "action", HelpText = "Action to perform on repository group e.g. add")]
        public RepositoryGroupAction Action { get; set; }
        
    }
}