using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AProjectManager.Git.Models;
using Avoid.Cli;

namespace AProjectManager.Git
{
    public class RepositoryManager
    {
        public static IProcess Clone(Clone clone)
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

        public static IProcess Fetch(LocalRepository localRepository)
        {
            return ChangeDirGitProcess(localRepository, "fetch");
        }

        public static IProcess Pull(LocalRepository localRepository)
        {
            return ChangeDirGitProcess(localRepository, "pull");
        }

        public static IProcess Checkout(LocalRepository localRepository, Checkout checkout)
        {
            var originalDirectory = Directory.GetCurrentDirectory();
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument("checkout");

                if (checkout.Create)
                {
                    b.AddFlagArgument("-b", checkout.Branch.Name);
                }
                else
                {
                    b.AddArgument(checkout.Branch.Name);
                }

                b.AddPreprocessAction(action => Directory.SetCurrentDirectory(localRepository.Location));
                b.AddPostprocessAction(action => Directory.SetCurrentDirectory(originalDirectory));
            });

            return process;
        }

        public static IProcess Branch(LocalRepository localRepository, IEnumerable<Action<object, DataReceivedEventArgs>> dataReceivedCallback)
        {
            var process = ChangeDirGitProcess(localRepository, "branch", b =>
            {
                b.AddFlag("-a");

                foreach (var action in dataReceivedCallback)
                {
                    b.AddDataReceivedCallback(action);
                }
            });

            return process;
        }

        private static IProcess ChangeDirGitProcess(IRepository localRepository, string gitAction, params Action<IBuilderActions>[] extraActions)
        {
            var originalDirectory = Directory.GetCurrentDirectory();
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument(gitAction);

                foreach (var action in extraActions)
                {
                    action.Invoke(b);
                }
                
                b.AddPreprocessAction(action => Directory.SetCurrentDirectory(localRepository.Location));
                b.AddPostprocessAction(action => Directory.SetCurrentDirectory(originalDirectory));
            });

            return process;
        }
    }
}