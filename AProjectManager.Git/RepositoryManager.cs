using System.Collections.Generic;
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
            var originalDirectory = Directory.GetCurrentDirectory();
            
            var programBuilder = new CliProgramBuilder();

            var process = programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument("fetch");
                b.AddPreprocessAction(action => Directory.SetCurrentDirectory(localRepository.Location));
                b.AddPostprocessAction(action => Directory.SetCurrentDirectory(originalDirectory));
            });

            return process;
        }
    }
}