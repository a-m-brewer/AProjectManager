using System.Threading.Tasks;
using AProjectManager.BitBucket.Models;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface ILoginManager
    {
        Task<TokenDto> Login(AuthorizationCredentials authorizationCredentials);
        User GetUser();
        User RegisterUser(string username, string password);
    }
}