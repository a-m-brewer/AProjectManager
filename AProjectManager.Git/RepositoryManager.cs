using System.IO;
using AProjectManager.Domain.Git;
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

        private static IProcess ChangeDirGitProcess(IRepository localRepository, string gitAction)
        {
            var originalDirectory = Directory.GetCurrentDirectory();
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument(gitAction);
                b.AddPreprocessAction(action => Directory.SetCurrentDirectory(localRepository.Location));
                b.AddPostprocessAction(action => Directory.SetCurrentDirectory(originalDirectory));
            });

            return process;
        }
    }
}