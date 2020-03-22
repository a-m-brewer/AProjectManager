using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AProjectManager.Cli.CliServices;
using AProjectManager.Cli.Verbs;
using CommandLine;

namespace AProjectManager.Cli
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var tasks = new List<Task>();

            Parser.Default.ParseArguments<LoginVerb, CloneVerb, GroupVerb, SessionVerb, DockerComposeVerb, RepositorySourceVerb, PrintVerb, GitVerb>(args)
                .WithParsed<LoginVerb>(verb => tasks.Add(new LoginService().Run(verb)))
                .WithParsed<CloneVerb>(verb => tasks.Add(new CloneService().Run(verb)))
                .WithParsed<GroupVerb>(verb => tasks.Add(new RepositoryGroupService().Run(verb)))
                .WithParsed<SessionVerb>(verb => tasks.Add(new RepositorySessionService().Run(verb)))
                .WithParsed<DockerComposeVerb>(verb => tasks.Add(new DockerComposeService().Run(verb)))
                .WithParsed<RepositorySourceVerb>(verb => tasks.Add(new RepositorySourceService().Run(verb)))
                .WithParsed<PrintVerb>(verb => tasks.Add(new PrintService().Run(verb)))
                .WithParsed<GitVerb>(verb => tasks.Add(new GitService().Run(verb)))
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                });
               

            await Task.WhenAll(tasks);
        }
    }
}