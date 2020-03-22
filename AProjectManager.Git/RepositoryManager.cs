using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AProjectManager.Git.Models;
using Avoid.Cli;

namespace AProjectManager.Git
{
    public static class RepositoryManager
    {
        public static IProcess Clone(this Clone clone)
        {
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument("clone");
                b.AddArgument(clone.Remote.Location);
                b.AddArgument(clone.Local.Location);
            });

            return process;
        }

        public static IProcess Fetch(this LocalRepository localRepository)
        {
            return ChangeDirGitProcess(localRepository.Location, "fetch");
        }

        public static IProcess Pull(this LocalRepository localRepository)
        {
            return ChangeDirGitProcess(localRepository.Location, "pull");
        }

        public static IProcess Checkout(this LocalRepository localRepository, Checkout checkout)
        {
            return ChangeDirGitProcess(localRepository.Location, "checkout", b =>
            {
                if (checkout.Create)
                {
                    b.AddFlagArgument("-b", checkout.Branch.Name);
                }
                else
                {
                    b.AddArgument(checkout.Branch.Name);
                }
            });
        }

        public static IProcess Branch(this LocalRepository localRepository, params Action<object, DataReceivedEventArgs>[] dataReceivedCallbacks)
        {
            var process = ChangeDirGitProcess(localRepository.Location, "branch", b =>
            {
                b.AddFlag("-a");

                foreach (var action in dataReceivedCallbacks)
                {
                    b.AddDataReceivedCallback(action);
                }
            });

            return process;
        }

        public static IProcess RevParse(this LocalRepository localRepository,
            params Action<object, DataReceivedEventArgs>[] dataReceivedCallbacks)
        {
            var process = ChangeDirGitProcess(localRepository.Location, "rev-parse", b =>
            {
                b.AddFlagArgument("--abbrev-ref", "HEAD");

                foreach (var action in dataReceivedCallbacks)
                {
                    b.AddDataReceivedCallback(action);
                }
            });

            return process;
        }

        public static IProcess Log(this LocalRepository localRepository,
            params Action<object, DataReceivedEventArgs>[] dataReceivedCallbacks)
        {
            return ChangeDirGitProcess(localRepository.Location, "log", b =>
            {
                b.AddFlag("--pretty=%B");
                foreach (var action in dataReceivedCallbacks)
                {
                    b.AddDataReceivedCallback(action);
                }
            });
        }

        public static IProcess Super(this LocalRepository localRepository, IEnumerable<string> arguments)
        {
            return ChangeDirGitProcess(localRepository.Location, b =>
            {
                foreach (var argument in arguments)
                {
                    b.AddArgument(argument, false);
                    b.BuildArgumentsInAddOrder();
                    b.AddDataReceivedCallback((o, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data) && !string.IsNullOrWhiteSpace(args.Data))
                        {
                            Console.WriteLine(args.Data);
                        }
                    });
                }
            });
        }
        
        public static IProcess Config(string localRepositoryLocation,
            params Action<object, DataReceivedEventArgs>[] dataReceivedCallback)
        {
            var process = ChangeDirGitProcess(localRepositoryLocation, "config", b =>
            {
                b.AddFlagArgument("--get", "remote.origin.url");

                foreach (var callback in dataReceivedCallback)
                {
                    b.AddDataReceivedCallback(callback);
                }
            });

            return process;
        }

        private static IProcess ChangeDirGitProcess(string localRepositoryLocation, string gitAction, Action<IBuilderActions> extraAction)
        {
            return ChangeDirGitProcess(localRepositoryLocation, b =>
            {
                b.AddArgument(gitAction);
                extraAction.Invoke(b);
            });
        }

        private static IProcess ChangeDirGitProcess(string localRepositoryLocation, string gitAction)
        {
            return ChangeDirGitProcess(localRepositoryLocation, gitAction, b => { });
        }
        
        private static IProcess ChangeDirGitProcess(string localRepositoryLocation, Action<IBuilderActions> extraAction)
        {
            var originalDirectory = Directory.GetCurrentDirectory();
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                
                extraAction.Invoke(b);
                
                b.AddPreprocessAction(action => Directory.SetCurrentDirectory(localRepositoryLocation));
                b.AddPostprocessAction(action => Directory.SetCurrentDirectory(originalDirectory));
            });

            return process;
        }
    }
}