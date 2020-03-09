using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Domain.BitBucket;
using AProjectManager.Persistence.FileData;

namespace AProjectManager
{
    public class BitBucketLoginManager : ILoginManager
    {
        private readonly IFileConfigManager _fileConfigManager;
        private readonly IBitBucketClient _client;

        public BitBucketLoginManager(
            IFileConfigManager fileConfigManager,
            IBitBucketClient client)
        {
            _fileConfigManager = fileConfigManager;
            _client = client;
        }
        
        public async Task Login(AuthorizationCredentials authorizationCredentials)
        {
            var existingToken = _fileConfigManager.GetFromFile<TokenDto>(ConfigFiles.Token);

            if (existingToken == null)
            {
                var newToken = await _client.Authorize(authorizationCredentials);
                _fileConfigManager.WriteData(newToken.ToPersistenceToken(), ConfigFiles.Token);
                return;
            }

            if (!existingToken.Expired)
            {
                return;
            }
            
            var refreshedToken = await _client.Authorize(new RefreshTokenRequest
            {
                AuthorizationCredentials = authorizationCredentials,
                RefreshToken = existingToken.Token.RefreshToken
            });
            
            _fileConfigManager.WriteData(refreshedToken.ToPersistenceToken(), ConfigFiles.Token);
        }
    }
}