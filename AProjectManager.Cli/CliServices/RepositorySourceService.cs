using System;
using System.Threading.Tasks;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Enums;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Constants;
using AProjectManager.Interfaces;
using Autofac;
using CommandLine.Text;

namespace AProjectManager.Cli.CliServices
{
    public class RepositorySourceService : ICliService<RepositorySourceVerb>
    {
        public async Task Run(RepositorySourceVerb verb)
        {
            using var scopeFactory = new ScopeFactory(Services.RepositorySourceService);
            var scope = scopeFactory.Scope;
            
            var repositorySourceManager = scope.Resolve<IRepositorySourceManager>();

            switch (verb.Actions)
            {
                case RepositoryActions.Add:
                    await repositorySourceManager.Add(verb.ToAddRequest());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}