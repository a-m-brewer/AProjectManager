using CommandLine;

namespace AProjectManager.Cli.Verbs
{
    [Verb("login", HelpText = "Login to a remote service like BitBucket")]
    public class LoginVerb : HelperOptions
    {
        [Option('u', "username", HelpText = "User name for online account e.g. bitbucket OAuth Key")]
        public string UserName { get; set; }
        
        [Option('p', "password", HelpText = "Password for online account e.g. bitbucket OAuth Secret")]
        public string Password { get; set; }
    }
}