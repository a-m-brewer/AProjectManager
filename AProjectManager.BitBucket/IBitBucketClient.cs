using System.Threading.Tasks;
using AProjectManager.Domain.BitBucket;

namespace AProjectManager.BitBucket
{
    public interface IBitBucketClient
    {
        Task<TokenDto> Authorize(AuthorizationCredentials authorizationCredentials);
        Task<TokenDto> Authorize(RefreshTokenRequest refreshTokenRequest);
    }
}