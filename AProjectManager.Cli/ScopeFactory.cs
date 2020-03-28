using System;
using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Cli.ConsoleServices;
using AProjectManager.Cli.Events;
using AProjectManager.Cli.Interfaces;
using AProjectManager.Constants;
using AProjectManager.Interfaces;
using AProjectManager.Managers;
using AProjectManager.Managers.BitBucket;
using AProjectManager.Persistence.FileData;
using AProjectManager.Repositories;
using Autofac;

namespace AProjectManager.Cli
{
    public class ScopeFactory : IDisposable
    {
        public ILifetimeScope Scope { get; }

        public ScopeFactory(string service)
        {
            var container = BuildContainer(service);
            Scope = container.BeginLifetimeScope();
        }

        private static IContainer BuildContainer(string service)
        {
            var containerBuilder = new ContainerBuilder();
            RegisterConfigClasses(containerBuilder);
            RegisterManagers(containerBuilder);
            RegisterLoginManager(service, containerBuilder);
            RegisterCliSpecificClasses(containerBuilder);
            return containerBuilder.Build();
        }

        private static void RegisterManagers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RepositoryGroupManager>().As<IRepositoryGroupManager>();
            containerBuilder.RegisterType<RepositorySessionManager>().As<IRepositorySessionManager>();
            containerBuilder.RegisterType<RepositoryRegisterManager>().As<IRepositoryRegisterManager>();
            containerBuilder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            containerBuilder.RegisterType<DockerComposeManager>().As<IDockerComposeManager>();
            containerBuilder.RegisterType<PrintManager>().As<IPrintManager>();
            containerBuilder.RegisterType<GitManager>().As<IGitManager>();
            containerBuilder.RegisterType<TablePrinterService>().As<ITablePrinterService>();
            containerBuilder.RegisterType<RepositorySourceManager>().As<IRepositorySourceManager>();
        }

        private static void RegisterConfigClasses(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataFolderProvider>().As<IDataFolderProvider>();
            containerBuilder.RegisterType<FileManager>().As<IFileManager>();
            containerBuilder.RegisterType<YamlConfigManager>().As<IConfigManager>();
            containerBuilder.RegisterType<FileConfigManager>().As<IFileConfigManager>();
            containerBuilder.RegisterType<FileRepository>().As<IFileRepository>();
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

        private static void RegisterCliSpecificClasses(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ConsoleContinueEvent>().As<IContinueEvent>();
        }

        public void Dispose()
        {
            Scope?.Dispose();
        }
    }
}