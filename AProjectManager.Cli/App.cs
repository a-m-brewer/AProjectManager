using System;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Domain.BitBucket;
using AProjectManager.Persistence.FileData;
using Autofac;
using Autofac.Core;

namespace AProjectManager.Cli
{
    public class App
    {
        public async Task Run(CliArguments arguments)
        {
            var container = BuildDiContainer(arguments);
            
            if (!string.IsNullOrEmpty(arguments.Login))
            {
                await using var scope = container.BeginLifetimeScope();
                var loginManager = scope.Resolve<ILoginManager>();
                var user = GetUser(arguments);
                await loginManager.Login(new AuthorizationCredentials {Key = user.Username, Secret = user.Password});
            }
        }

        private User GetUser(CliArguments arguments)
        {
            var username = arguments.UserName ?? ReadLine.Read("Username: ");
            var password = arguments.Password ?? ReadLine.ReadPassword("Password: ");
            
            return new User
            {
                Username = username,
                Password = password
            };
        }

        private IContainer BuildDiContainer(CliArguments cliArguments)
        {
            var containerBuilder = new ContainerBuilder();
            
            RegisterConfigManager(containerBuilder);
            RegisterLoginManager(cliArguments, containerBuilder);

            return containerBuilder.Build();
        }

        private static void RegisterConfigManager(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataFolderProvider>().As<IDataFolderProvider>();
            containerBuilder.RegisterType<FileManager>().As<IFileManager>();
            containerBuilder.RegisterType<YamlConfigManager>().As<IConfigManager>();
            containerBuilder.RegisterType<FileConfigManager>().As<IFileConfigManager>();
        }
        
        private static void RegisterLoginManager(CliArguments cliArguments, ContainerBuilder containerBuilder)
        {
            switch (cliArguments.Login.ToLowerInvariant())
            {
                case LoginOptions.BitBucket:
                    containerBuilder.RegisterType<BitBucketClient>().As<IBitBucketClient>();
                    containerBuilder.RegisterType<BitBucketLoginManager>().As<ILoginManager>();
                    break;
                default:
                    break;
            }
        }
    }
}