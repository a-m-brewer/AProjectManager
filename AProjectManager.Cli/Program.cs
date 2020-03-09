using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace AProjectManager.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tasks = new List<Task>();
            
            var testArgs = new[]
                {"-l", "bitbucket", "-u", "mkuRcCtmt7LTRHn95x", "-p", "rB9vkEMEwbb89Er38t2cEAFrawn2Gbut"};
            
            Parser.Default.ParseArguments<CliArguments>(testArgs)
                .WithParsed(passed => tasks.Add(new App().Run(passed)))
                .WithNotParsed(failure => new AppError().HandleErrors(failure));

            await Task.WhenAll(tasks);
        }
    }
}