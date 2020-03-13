using System;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Cli.Converters;
using AProjectManager.Cli.Models;
using AProjectManager.Domain.BitBucket;
using AProjectManager.Enums;
using AProjectManager.Extensions;
using AProjectManager.Interfaces;
using AProjectManager.Models;
using AProjectManager.Persistence.FileData;
using Autofac;
using Autofac.Core;

namespace AProjectManager.Cli
{
    public class App
    {
        public async Task Login(LoginVerb arguments)
        {
            var container = BuildContainer(arguments.Service);
            
            await using var scope = container.BeginLifetimeScope();
            var loginManager = scope.Resolve<ILoginManager>();
            
            var user = GetUser(loginManager, arguments.UserName, arguments.Password);
            
            await loginManager.Login(user.ToAuthCredentials());
        }

        public async Task Clone(CloneVerb arguments)
        {
            var container = BuildContainer(arguments.Service);
            
            await using var scope = container.BeginLifetimeScope();
            var loginManager = scope.Resolve<ILoginManager>();

            var user = loginManager.GetUser();

            if (user == null)
            {
                Console.WriteLine("Could not find user, please use login before cloning repositories");
            }

            var login = await loginManager.Login(user.ToAuthCredentials());

            var cloneManager = scope.Resolve<ICloneManager>();

            await cloneManager.Clone(new CloneRequest
            {
                CloneDirectory = arguments.CloneDirectory,
                GetRepositoriesRequest = arguments.ToGetRepositoriesRequest(login.Token.AccessToken)
            });
        }

        public async Task RepositoryGroup(GroupVerb group)
        {
            var container = BuildContainer(Services.RepositoryGroupService);
            
            await using var scope = container.BeginLifetimeScope();
            var repositoryGroupManager = scope.Resolve<IRepositoryGroupManager>();

            switch (group.Action)
            {
                case RepositoryGroupAction.Add:
                    var groupResult = await repositoryGroupManager.Add(group.ToAdd());
                    break;
            }
        }

        private User GetUser(ILoginManager loginManager, string userName, string password)
        {
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
            return containerBuilder.Build();
        }

        private static void RegisterConfigManager(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataFolderProvider>().As<IDataFolderProvider>();
            containerBuilder.RegisterType<FileManager>().As<IFileManager>();
            containerBuilder.RegisterType<YamlConfigManager>().As<IConfigManager>();
            containerBuilder.RegisterType<FileConfigManager>().As<IFileConfigManager>();
            containerBuilder.RegisterType<RepositoryGroupManager>().As<IRepositoryGroupManager>();
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