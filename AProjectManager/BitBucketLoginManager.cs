using System.Threading.Tasks;
using AProjectManager.BitBucket;
using AProjectManager.Constants;
using AProjectManager.Domain.BitBucket;
using AProjectManager.Interfaces;
using AProjectManager.Models;
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

        public async Task<TokenDto> Login(AuthorizationCredentials authorizationCredentials)
        {
            var existingToken = _fileConfigManager.GetFromFile<TokenDto>(ConfigFiles.Token);

            if (existingToken == null)
            {
                var newToken = await _client.Authorize(authorizationCredentials);
                _fileConfigManager.WriteData(newToken.ToPersistenceToken(), ConfigFiles.Token);
                return newToken;
            }

            if (!existingToken.Expired)
            {
                return existingToken;
            }
            
            var refreshedToken = await _client.AuthorizeAsync(new RefreshTokenRequest
            {
                AuthorizationCredentials = authorizationCredentials,
                RefreshToken = existingToken.Token.RefreshToken
            });
            
            _fileConfigManager.WriteData(refreshedToken.ToPersistenceToken(), ConfigFiles.Token);

            return refreshedToken;
        }

        public User GetUser()
        {
            var existingUser = _fileConfigManager.GetFromFile<User>(ConfigFiles.User);
            return existingUser;
        }

        public User RegisterUser(string username, string password)
        {
            return _fileConfigManager.WriteData(new User
            {
                Username = username,
                Password = password
            }, ConfigFiles.User);
        }
    }
}