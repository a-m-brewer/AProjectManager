using AProjectManager.Constants;
using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("print", HelpText = "Print information about repositories groups and sessions")]
    public class PrintVerb
    {
        [Option('t', "type", HelpText = "The type of resource e.g. Session you want to print", Default = ItemType.All)]
        public ItemType Type { get; set; }
        [Option('n', "name", HelpText = "The name of the resource")]
        public string Name { get; set; }
    }
}