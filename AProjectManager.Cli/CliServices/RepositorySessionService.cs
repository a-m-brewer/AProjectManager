using System;
using System.Threading.Tasks;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Enums;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Constants;
using AProjectManager.Interfaces;
using Autofac;

namespace AProjectManager.Cli.CliServices
{
    public class RepositorySessionService : ICliService<SessionVerb>
    {
        public async Task Run(SessionVerb verb)
        { 
            using var scopeFactory = new ScopeFactory(Services.RepositorySessionService);
            var scope = scopeFactory.Scope;
            
            var sessionManager = scope.Resolve<IRepositorySessionManager>();

            switch (verb.Action)
            {
                case RepositorySessionAction.Start:
                    Console.WriteLine($"Starting session: {verb.BranchName}");
                    await sessionManager.Start(verb.ToSessionStartRequest());
                    Console.WriteLine($"Started session: {verb.BranchName}");
                    break;
                case RepositorySessionAction.Checkout:
                    Console.WriteLine($"Checking out existing session: {verb.BranchName}"); 
                    await sessionManager.Checkout(verb.ToSessionCheckoutRequest());
                    Console.WriteLine($"Checked out existing session: {verb.BranchName}");
                    break;
                case RepositorySessionAction.Exit:
                    Console.WriteLine($"Exiting out of session: {verb.BranchName}"); 
                    await sessionManager.Exit(verb.ToSessionExitRequest());
                    Console.WriteLine($"Exited out of session: {verb.BranchName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}