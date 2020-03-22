using System;
using System.Threading.Tasks;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using Autofac;
using CommandLine.Text;

namespace AProjectManager.Cli.CliServices
{
    public class LoginService : ICliService<LoginVerb>
    {
        public async Task Run(LoginVerb verb)
        {
            using var scopeFactory = new ScopeFactory(verb.Service);
            var scope = scopeFactory.Scope;
            var loginManager = scope.Resolve<ILoginManager>();
            
            var user = GetUser(loginManager, verb.UserName, verb.Password);
            
            await loginManager.Login(user.ToAuthCredentials());
        }
        
        private static User GetUser(ILoginManager loginManager, string userName, string password)
        {
            var existingUser = loginManager.GetUser();

            if (existingUser != null)
            {
                return existingUser;
            }
            
            var uname = userName ?? ReadLine.Read("Username: ");
            var pword = password ?? ReadLine.ReadPassword("Password: ");
            
            return loginManager.RegisterUser(
                uname,
                pword);
        }
    }
}