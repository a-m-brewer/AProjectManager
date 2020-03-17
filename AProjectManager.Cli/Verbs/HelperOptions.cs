using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    public class HelperOptions
    {
        [Option('s', "service", HelpText = "Service to execute command with")]
        public string Service { get; set; }
    }
}