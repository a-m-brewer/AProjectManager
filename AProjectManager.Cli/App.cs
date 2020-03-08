using System;
using System.Threading.Tasks;

namespace AProjectManager.Cli
{
    public class App
    {
        public async Task Run(CliArguments arguments)
        {
            if (!string.IsNullOrEmpty(arguments.Login))
            {
                switch (arguments.Login.ToLowerInvariant())
                {
                    case LoginOptions.BitBucket:
                        // TODO: use DI to get instance of bitbucket login manager.
                        break;
                    default:
                        Console.WriteLine($"{arguments.Login} is not a supported login option");
                        break;
                }
            }
        }

        private User GetUser(CliArguments arguments)
        {
            var username = arguments.UserName ?? ReadLine.Read("Username: ");
            var password = arguments.Password ?? ReadLine.ReadPassword("Password: ");
            
            return new User
            {
                Username = username,
                Password = password
            };
        }
    }
}