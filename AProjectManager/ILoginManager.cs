using System.Threading.Tasks;
using AProjectManager.Domain.BitBucket;

namespace AProjectManager
{
    public interface ILoginManager
    {
        Task Login(AuthorizationCredentials authorizationCredentials);
    }
    
    public interface IBitBucketLoginManager : ILoginManager {}
}