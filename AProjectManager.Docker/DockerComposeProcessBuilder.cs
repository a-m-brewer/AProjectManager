using System; 
using System.Diagnostics;
using AProjectManager.Docker.Models;
using Avoid.Cli;

namespace AProjectManager.Docker
{
    public static class DockerComposeProcessBuilder
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
                b.AddDataReceivedCallback(Print);
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
                b.AddDataReceivedCallback(Print);
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
                b.AddDataReceivedCallback(Print);
            });

            return process;   
        }

        public static void Print(object o, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}