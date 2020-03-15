using AProjectManager.BitBucket.Models;
using AProjectManager.Models;

namespace AProjectManager.Extensions
{
    public static class UserExtensions
    {
        public static AuthorizationCredentials ToAuthCredentials(this User user)
        {
            return new AuthorizationCredentials
            {
                Key = user.Username,
                Secret = user.Password
            };
        }
    }
}