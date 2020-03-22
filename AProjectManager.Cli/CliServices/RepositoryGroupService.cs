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
    public class RepositoryGroupService : ICliService<GroupVerb>
    {
        public async Task Run(GroupVerb verb)
        { 
            using var scopeFactory = new ScopeFactory(Services.RepositoryGroupService);
            var scope = scopeFactory.Scope;
            
            var repositoryGroupManager = scope.Resolve<IRepositoryGroupManager>();

            switch (verb.Action)
            {
                case RepositoryGroupAction.Add:
                    Console.WriteLine($"Adding group: {verb.GroupName}");
                    var groupResult = await repositoryGroupManager.Add(verb.ToAdd());
                    Console.WriteLine($"Added group: {verb.GroupName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}