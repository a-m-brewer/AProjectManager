using System;
using System.Threading.Tasks;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Constants;
using AProjectManager.Interfaces;
using Autofac;

namespace AProjectManager.Cli.CliServices
{
    public class PrintService : ICliService<PrintVerb>
    {
        public async Task Run(PrintVerb verb)
        { 
            using var scopeFactory = new ScopeFactory(Services.PrintService);
            var scope = scopeFactory.Scope;
            
            var printManager = scope.Resolve<IPrintManager>();
            var tablePrinterService = scope.Resolve<ITablePrinterService>();

            switch (verb.Type)
            {
                case ItemType.Repository:
                    Console.WriteLine(nameof(ItemType.Repository));
                    var repos = printManager.GetRepositoryData();
                    tablePrinterService.Print(repos);
                    break;
                case ItemType.Group:
                    Console.WriteLine(nameof(ItemType.Group));
                    var groups = printManager.GetGroupData();
                    tablePrinterService.Print(groups);
                    break;
                case ItemType.Session:
                    Console.WriteLine(nameof(ItemType.Session));
                    var sessions = printManager.GetSessionData();
                    tablePrinterService.Print(sessions);
                    break;
                case ItemType.Source:
                    Console.WriteLine(nameof(ItemType.Source));
                    var sources = printManager.GetRepositorySourcesData();
                    tablePrinterService.Print(sources);
                    break;
                case ItemType.All:
                    Console.WriteLine(nameof(ItemType.Source));
                    var sourcesAll = printManager.GetRepositorySourcesData();
                    tablePrinterService.Print(sourcesAll);
                    Console.WriteLine(nameof(ItemType.Repository));
                    var reposAll = printManager.GetRepositoryData();
                    tablePrinterService.Print(reposAll);
                    Console.WriteLine(nameof(ItemType.Group));
                    var groupsAll = printManager.GetGroupData();
                    tablePrinterService.Print(groupsAll);
                    Console.WriteLine(nameof(ItemType.Session));
                    var sessionsAll = printManager.GetSessionData();
                    tablePrinterService.Print(sessionsAll);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}