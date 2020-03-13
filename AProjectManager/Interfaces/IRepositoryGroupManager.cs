using System.Threading.Tasks;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IRepositoryGroupManager
    {
        Task<RepositoryGroup> Add(GroupAddRequest groupAddRequest);
    }
}