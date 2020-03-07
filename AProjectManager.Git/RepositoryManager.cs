using System.Collections.Generic;
using AProjectManager.Domain.Git;
using Avoid.Cli;

namespace AProjectManager.Git
{
    public class RepositoryManager
    {
        public IRunnableProcess Clone(Clone clone)
        {
            var programBuilder = new CliProgramBuilder();

            var process =programBuilder.Build(b =>
            {
                b.AddProgram("git");
                b.AddArgument("clone");
                b.AddArgument(clone.Remote.Location);
                b.AddArgument(clone.Local.Location);
            });
            
            return new AggregateRunnableProcess(new List<IRunnableProcess>
            {
                process   
            });
        }
    }
}