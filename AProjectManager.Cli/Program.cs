using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AProjectManager.Cli.Models;
using CommandLine;

namespace AProjectManager.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tasks = new List<Task>();
            
            var app = new App();

            Parser.Default.ParseArguments<LoginVerb, CloneVerb, GroupVerb, SessionVerb>(args)
                .WithParsed<LoginVerb>(verb => tasks.Add(app.Login(verb)))
                .WithParsed<CloneVerb>(verb => tasks.Add(app.Clone(verb)))
                .WithParsed<GroupVerb>(verb => tasks.Add(app.RepositoryGroup(verb)))
                .WithParsed<SessionVerb>(verb => tasks.Add(app.RepositorySession(verb)))
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