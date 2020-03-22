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
    public class DockerComposeService : ICliService<DockerComposeVerb>
    {
        public async Task Run(DockerComposeVerb verb)
        {
            using var scopeFactory = new ScopeFactory(Services.DockerComposeService);
            var scope = scopeFactory.Scope;
            
            var dockerComposeManager = scope.Resolve<IDockerComposeManager>();
            
            switch (verb.DockerAction)
            {
                case DockerAction.Up:
                    Console.WriteLine($"Starting Docker Containers for {verb.Name} using {verb.FileName} build: {verb.Build}");
                    await dockerComposeManager.Up(verb.ToDockerComposeUpRequest());
                    Console.WriteLine($"Stated Docker Containers for {verb.Name} using {verb.FileName} build: {verb.Build}");
                    break;
                case DockerAction.Down:
                    Console.WriteLine($"Stopping Docker Containers for {verb.Name} using {verb.FileName}");
                    await dockerComposeManager.Down(verb.ToDockerComposeDownRequest());
                    Console.WriteLine($"Stoped Docker Containers for {verb.Name} using {verb.FileName}");
                    break;
                case DockerAction.Super:
                    Console.WriteLine($"Running Super Command for {verb.Name} using {verb.FileName}");
                    await dockerComposeManager.Super(verb.ToDockerComposeSuperRequest());
                    Console.WriteLine($"Ran Super Command for {verb.Name} using {verb.FileName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}