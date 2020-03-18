using System.Collections.Generic;
using System.Threading.Tasks;
using AProjectManager.Managers;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositorySourceManager
    {
        Task<List<RepositorySource>> Add(RepositorySourceAddRequest request);
    }
}