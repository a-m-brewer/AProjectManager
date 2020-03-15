using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AProjectManager.BitBucket.Models;

namespace AProjectManager.BitBucket
{
    public interface IBitBucketClient
    {
        Task<TokenDto> Authorize(AuthorizationCredentials authorizationCredentials, CancellationToken cancellationToken = default);
        Task<TokenDto> AuthorizeAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default);

        Task<List<ApiRepo>> GetRepositoriesAsync(GetRepositoriesRequest getRepositoriesRequest,
            CancellationToken cancellationToken = default);
    }
}