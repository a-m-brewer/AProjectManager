using System.Collections.Generic;
using AProjectManager.Git.Models;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositoryRegisterManager
    {
        RepositoryRegister UpdateRegister(string repositoriesFileName);
        RepositoryRegister GetRegister();
    }
}