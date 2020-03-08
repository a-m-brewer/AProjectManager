using System;

namespace AProjectManager.Domain.BitBucket
{
    public class TokenDto : PersistenceToken
    {
        public DateTime ExpirationDateTime => TimeAuthenticated.AddSeconds(Token.ExpiresIn);
        public bool Expired => ExpirationDateTime < DateTime.UtcNow;

        public PersistenceToken ToPersistenceToken()
        {
            return new PersistenceToken
            {
                Token = Token,
                TimeAuthenticated = TimeAuthenticated
            };
        }
    }
}