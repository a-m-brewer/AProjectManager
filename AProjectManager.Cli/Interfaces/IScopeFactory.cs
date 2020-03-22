using System;
using System.Threading.Tasks;
using Autofac;

namespace AProjectManager.Cli.Interfaces
{
    public interface IScopeFactory
    {
        Task<ILifetimeScope> GetScope(string service);
    }
}