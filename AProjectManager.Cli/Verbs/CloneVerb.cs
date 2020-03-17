using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("clone", HelpText = "Clone Repositories from a remote service")]
    public class CloneVerb : HelperOptions
    {
        [Option('u', "user", HelpText = "User to check the repositories against. This also works with team names")]
        public string User { get; set; }
        [Option('r', "role", HelpText = "The role of the user in the repositories")]
        public string Role { get; set; }

        [Option('p', "project", HelpText = "The key of the project")]
        public string ProjectKey { get; set; }

        [Option('d', "clone-directory", HelpText = "Where to Clone the Repositories to")]
        public string CloneDirectory { get; set; }
    }
}