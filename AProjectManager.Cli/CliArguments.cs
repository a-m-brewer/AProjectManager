using CommandLine;

namespace AProjectManager.Cli
{
    public class CliArguments
    {
        [Option('l', "login", HelpText = "Login to Online Account e.g. --login bitbucket")]
        public string Login { get; set; }

        [Option('u', "username", HelpText = "User name for online account e.g. bitbucket OAuth Key")]
        public string UserName { get; set; }
        
        [Option('p', "password", HelpText = "Password for online account e.g. bitbucket OAuth Secret")]
        public string Password { get; set; }
    }
}