using System;
using System.Linq;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Cli.ConsoleServices;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Enums;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Cli.Verbs;
using AProjectManager.Constants;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Managers;
using AProjectManager.Managers.BitBucket;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using AProjectManager.Repositories;
using AProjectManager.Utils;
using Autofac;
using CommandLine.Text;

namespace AProjectManager.Cli
{
    public class App
    {
        public async Task Login(LoginVerb arguments)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(arguments.Service);
            
            await using var scope = container.BeginLifetimeScope();
            var loginManager = scope.Resolve<ILoginManager>();
            
            var user = GetUser(loginManager, arguments.UserName, arguments.Password);
            
            await loginManager.Login(user.ToAuthCredentials());
        }

        public async Task Clone(CloneVerb arguments)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(arguments.Service);
            
            await using var scope = container.BeginLifetimeScope();
            var loginManager = scope.Resolve<ILoginManager>();

            var user = loginManager.GetUser();

            if (user == null)
            {
                Console.WriteLine("Could not find user, please use login before cloning repositories");
                return;
            }
            
            Console.WriteLine($"Found User");

            Console.WriteLine("Logging in");
            var login = await loginManager.Login(user.ToAuthCredentials());
            Console.WriteLine("Login Success");

            var cloneManager = scope.Resolve<ICloneManager>();

            Console.WriteLine($"Cloning Repositories into {arguments.CloneDirectory}");
            await cloneManager.Clone(new CloneRequest
            {
                CloneDirectory = arguments.CloneDirectory,
                GetRepositoriesRequest = arguments.ToGetRepositoriesRequest(login.Token.AccessToken)
            });
            Console.WriteLine($"Cloned Repositories into {arguments.CloneDirectory}");
        }

        public async Task RepositoryGroup(GroupVerb group)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(Services.RepositoryGroupService);
            
            await using var scope = container.BeginLifetimeScope();
            var repositoryGroupManager = scope.Resolve<IRepositoryGroupManager>();

            switch (group.Action)
            {
                case RepositoryGroupAction.Add:
                    Console.WriteLine($"Adding group: {group.GroupName}");
                    var groupResult = await repositoryGroupManager.Add(group.ToAdd());
                    Console.WriteLine($"Added group: {group.GroupName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public async Task RepositorySession(SessionVerb verb)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(Services.RepositoryGroupService);
            
            await using var scope = container.BeginLifetimeScope();
            var sessionManager = scope.Resolve<IRepositorySessionManager>();

            switch (verb.Action)
            {
                case RepositorySessionAction.Start:
                    Console.WriteLine($"Starting session: {verb.BranchName}");
                    await sessionManager.Start(verb.ToSessionStartRequest());
                    Console.WriteLine($"Started session: {verb.BranchName}");
                    break;
                case RepositorySessionAction.Checkout:
                    Console.WriteLine($"Checking out existing session: {verb.BranchName}"); 
                    await sessionManager.Checkout(verb.ToSessionCheckoutRequest());
                    Console.WriteLine($"Checked out existing session: {verb.BranchName}");
                    break;
                case RepositorySessionAction.Exit:
                    Console.WriteLine($"Exiting out of session: {verb.BranchName}"); 
                    await sessionManager.Exit(verb.ToSessionExitRequest());
                    Console.WriteLine($"Exited out of session: {verb.BranchName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task DockerCompose(DockerComposeVerb verb)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(Services.RepositoryGroupService);
            
            await using var scope = container.BeginLifetimeScope();
            var dockerComposeManager = scope.Resolve<IDockerComposeManager>();
            
            switch (verb.DockerAction)
            {
                case DockerAction.Up:
                    Console.WriteLine($"Starting Docker Containers for {verb.Name} using {verb.FileName} build: {verb.Build}");
                    await dockerComposeManager.Up(verb.ToDockerComposeUpRequest());
                    Console.WriteLine($"Stated Docker Containers for {verb.Name} using {verb.FileName} build: {verb.Build}");
                    break;
                case DockerAction.Down:
                    Console.WriteLine($"Stopping Docker Containers for {verb.Name} using {verb.FileName}");
                    await dockerComposeManager.Down(verb.ToDockerComposeDownRequest());
                    Console.WriteLine($"Stoped Docker Containers for {verb.Name} using {verb.FileName}");
                    break;
                case DockerAction.Super:
                    Console.WriteLine($"Running Super Command for {verb.Name} using {verb.FileName}");
                    await dockerComposeManager.Super(verb.ToDockerComposeSuperRequest());
                    Console.WriteLine($"Ran Super Command for {verb.Name} using {verb.FileName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task RepositorySource(RepositorySourceVerb repositorySourceVerb)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var container = BuildContainer(Services.RepositorySourceManager);
            
            await using var scope = container.BeginLifetimeScope();
            var repositorySourceManager = scope.Resolve<IRepositorySourceManager>();

            switch (repositorySourceVerb.Actions)
            {
                case RepositoryActions.Add:
                    await repositorySourceManager.Add(repositorySourceVerb.ToAddRequest());
                    break;
                case RepositoryActions.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task Print(PrintVerb printVerb)
        {
            Console.WriteLine(HeadingInfo.Default);

            var container = BuildContainer(Services.PrintService);

            await using var scope = container.BeginLifetimeScope();
            var printManager = scope.Resolve<IPrintManager>();
            var tablePrinterService = scope.Resolve<ITablePrinterService>();

            switch (printVerb.Type)
            {
                case ItemType.Repository:
                    Console.WriteLine(nameof(ItemType.Repository));
                    var repos = printManager.GetRepositoryData();
                    tablePrinterService.Print(repos);
                    break;
                case ItemType.RepositoryGroup:
                    Console.WriteLine(nameof(ItemType.RepositoryGroup));
                    var groups = printManager.GetGroupData();
                    tablePrinterService.Print(groups);
                    break;
                case ItemType.RepositorySession:
                    Console.WriteLine(nameof(ItemType.RepositorySession));
                    var sessions = printManager.GetSessionData();
                    tablePrinterService.Print(sessions);
                    break;
                case ItemType.RepositorySource:
                    Console.WriteLine(nameof(ItemType.RepositorySource));
                    var sources = printManager.GetRepositorySourcesData();
                    tablePrinterService.Print(sources);
                    break;
                case ItemType.All:
                    Console.WriteLine(nameof(ItemType.RepositorySource));
                    var sourcesAll = printManager.GetRepositorySourcesData();
                    tablePrinterService.Print(sourcesAll);
                    Console.WriteLine(nameof(ItemType.Repository));
                    var reposAll = printManager.GetRepositoryData();
                    tablePrinterService.Print(reposAll);
                    Console.WriteLine(nameof(ItemType.RepositoryGroup));
                    var groupsAll = printManager.GetGroupData();
                    tablePrinterService.Print(groupsAll);
                    Console.WriteLine(nameof(ItemType.RepositorySession));
                    var sessionsAll = printManager.GetSessionData();
                    tablePrinterService.Print(sessionsAll);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task Git(GitVerb verb)
        {
            Console.WriteLine(HeadingInfo.Default);
            var container = BuildContainer(Services.PrintService);
            await using var scope = container.BeginLifetimeScope();

            var gitManager = scope.Resolve<IGitManager>();

            switch (verb.Action)
            {
                case GitAction.Pull:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Pull(verb.Names);
                    }
                    else
                    {
                        await gitManager.Pull();
                    }
                    break;
                case GitAction.Fetch:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Fetch(verb.Names);
                    }
                    else
                    {
                        await gitManager.Fetch();
                    }
                    break;
                case GitAction.Checkout:
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Checkout(verb.Names, verb.Branch);
                    }
                    else
                    {
                        await gitManager.Checkout(verb.Branch);
                    }
                    break;
                case GitAction.Super:
                    var arguments = verb.Super.Split(" ");
                    if (verb.Names != null && verb.Names.Any())
                    {
                        await gitManager.Super(verb.Names, arguments);
                    }
                    else
                    {
                        await gitManager.Super(arguments);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private User GetUser(ILoginManager loginManager, string userName, string password)
        {
            Console.WriteLine(HeadingInfo.Default);
            
            var existingUser = loginManager.GetUser();

            if (existingUser != null)
            {
                return existingUser;
            }
            
            var uname = userName ?? ReadLine.Read("Username: ");
            var pword = password ?? ReadLine.ReadPassword("Password: ");
            
            return loginManager.RegisterUser(
                uname,
                pword);
        }
        
        private static IContainer BuildContainer(string service)
        {
            var containerBuilder = new ContainerBuilder();
            RegisterConfigManager(containerBuilder);
            RegisterLoginManager(service, containerBuilder);
            RegisterManagers(containerBuilder);
            return containerBuilder.Build();
        }

        private static void RegisterManagers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RepositoryGroupManager>().As<IRepositoryGroupManager>();
            containerBuilder.RegisterType<RepositorySessionManager>().As<IRepositorySessionManager>();
            containerBuilder.RegisterType<RepositoryRegisterManager>().As<IRepositoryRegisterManager>();
            containerBuilder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            containerBuilder.RegisterType<DockerComposeManager>().As<IDockerComposeManager>();
        }

        private static void RegisterConfigManager(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataFolderProvider>().As<IDataFolderProvider>();
            containerBuilder.RegisterType<FileManager>().As<IFileManager>();
            containerBuilder.RegisterType<YamlConfigManager>().As<IConfigManager>();
            containerBuilder.RegisterType<FileConfigManager>().As<IFileConfigManager>();
            containerBuilder.RegisterType<RepositorySourceManager>().As<IRepositorySourceManager>();
            containerBuilder.RegisterType<FileRepository>().As<IFileRepository>();
            containerBuilder.RegisterType<TablePrinterService>().As<ITablePrinterService>();
            containerBuilder.RegisterType<PrintManager>().As<IPrintManager>();
            containerBuilder.RegisterType<GitManager>().As<IGitManager>();
        }
        
        private static void RegisterLoginManager(string service, ContainerBuilder containerBuilder)
        {
            switch (service.ToLowerInvariant())
            {
                case Services.BitBucket:
                    containerBuilder.RegisterType<BitBucketClient>().As<IBitBucketClient>();
                    containerBuilder.RegisterType<BitBucketLoginManager>().As<ILoginManager>();
                    containerBuilder.RegisterType<BitBucketCloneManager>().As<ICloneManager>();
                    break;
                default:
                    break;
            }
        }
    }
}