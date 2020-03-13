using System.Collections.Generic;
using AProjectManager.Docker.Models;
using Avoid.Cli;

namespace AProjectManager.Docker
{
    public class DockerComposeManager
    {
        public static IProcess Up(DockerComposeUpRequest upRequest)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("docker-compose");
                b.AddFlagArgument("-f", upRequest.FullPath);
                b.AddArgument("up");

                if (upRequest.Build)
                {
                    b.AddFlag("--build");
                }

                if (upRequest.Daemon)
                {
                    b.AddArgument("-d");
                }
            });

            return process;
        }
        
        public static IProcess Down(DockerComposeDownRequest downRequest)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("docker-compose");
                b.AddFlagArgument("-f", downRequest.FullPath);
                b.AddArgument("down");
            });

            return process;
        }
    }
}