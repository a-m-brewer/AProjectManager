using System.Collections.Generic;
using AProjectManager.Docker.Models;
using Avoid.Cli;

namespace AProjectManager.Docker
{
    public class DockerComposeProcessBuilder
    {
        public static IProcess Up(DockerComposeArguments upRequest)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("docker-compose");
                b.AddFlagArgument("-f", upRequest.FullPath);
                b.AddArgument("up", false);

                if (upRequest.Build)
                {
                    b.AddFlag("--build");
                }

                if (upRequest.Daemon)
                {
                    b.AddFlag("-d");
                }

                b.BuildArgumentsInAddOrder();
            });

            return process;
        }
        
        public static IProcess Down(DockerComposeArguments downRequest)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("docker-compose");
                b.AddFlagArgument("-f", downRequest.FullPath);
                b.AddArgument("down", false);
                b.BuildArgumentsInAddOrder();
            });

            return process;
        }

        public static IProcess Super(string fullPath, params string[] arguments)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("docker-compose");
                b.AddFlagArgument("-f", fullPath);

                foreach (var argument in arguments)
                {
                    b.AddArgument(argument, false);
                }
                
                b.BuildArgumentsInAddOrder();
            });

            return process;   
        }
    }
}