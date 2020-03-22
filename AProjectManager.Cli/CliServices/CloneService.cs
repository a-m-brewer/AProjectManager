using System;
using System.Threading.Tasks;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using Autofac;

namespace AProjectManager.Cli.CliServices
{
    public class CloneService : ICliService<CloneVerb>
    {
        public async Task Run(CloneVerb verb)
        {
            using var scopeFactory = new ScopeFactory(verb.Service);
            var scope = scopeFactory.Scope;
            
            var loginManager = scope.Resolve<ILoginManager>();

            var user = loginManager.GetUser();

            if (user == null)
            {
                Console.WriteLine("Could not find user, please use login before cloning repositories");
                return;
            }
            
            Console.WriteLine($"Found User");

            Console.WriteLine("Logging in");
            var login = await loginManager.Login(user.ToAuthCredentials());
            Console.WriteLine("Login Success");

            var cloneManager = scope.Resolve<ICloneManager>();

            Console.WriteLine($"Cloning Repositories into {verb.CloneDirectory}");
            await cloneManager.Clone(new CloneRequest
            {
                CloneDirectory = verb.CloneDirectory,
                GetRepositoriesRequest = verb.ToGetRepositoriesRequest(login.Token.AccessToken)
            });
            Console.WriteLine($"Cloned Repositories into {verb.CloneDirectory}");
        }
    }
}