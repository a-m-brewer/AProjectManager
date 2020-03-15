using System;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Models;
using AProjectManager.Constants;
using AProjectManager.Enums;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
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
                    var session = await sessionManager.Start(verb.ToSessionStartRequest());
                    Console.WriteLine($"Started session: {verb.BranchName}");
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
        }

        private static void RegisterConfigManager(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataFolderProvider>().As<IDataFolderProvider>();
            containerBuilder.RegisterType<FileManager>().As<IFileManager>();
            containerBuilder.RegisterType<YamlConfigManager>().As<IConfigManager>();
            containerBuilder.RegisterType<FileConfigManager>().As<IFileConfigManager>();
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