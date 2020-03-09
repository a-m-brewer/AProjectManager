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
            
            Parser.Default.ParseArguments<CliArguments>(args)
                .WithParsed(passed => tasks.Add(new App().Run(passed)))
                .WithNotParsed(failure => new AppError().HandleErrors(failure));

            await Task.WhenAll(tasks);
        }
    }
}