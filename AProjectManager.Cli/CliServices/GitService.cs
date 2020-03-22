using System;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.Cli.Enums;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Constants;
using AProjectManager.Interfaces;
using Autofac;

namespace AProjectManager.Cli.CliServices
{
    public class GitService : ICliService<GitVerb>
    {
        public async Task Run(GitVerb verb)
        {
            using var scopeFactory = new ScopeFactory(Services.GitService);
            var scope = scopeFactory.Scope;

            var gitManager = scope.Resolve<IGitManager>();

            switch (verb.Action)
            {
                case GitAction.Pull:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Pull(verb.Names);
                    }
                    else
                    {
                        await gitManager.Pull();
                    }
                    break;
                case GitAction.Fetch:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Fetch(verb.Names);
                    }
                    else
                    {
                        await gitManager.Fetch();
                    }
                    break;
                case GitAction.Checkout:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Checkout(verb.Names, verb.Branch);
                    }
                    else
                    {
                        await gitManager.Checkout(verb.Branch);
                    }
                    break;
                case GitAction.Super:
                    var arguments = verb.Super.Split(" ");
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Super(verb.Names, arguments);
                    }
                    else
                    {
                        await gitManager.Super(arguments);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}