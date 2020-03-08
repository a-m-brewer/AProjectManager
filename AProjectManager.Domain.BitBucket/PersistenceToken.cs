using System;

namespace AProjectManager.Domain.BitBucket
{
    public class PersistenceToken
    {
        public Token Token { get; set; }
        public DateTime TimeAuthenticated { get; set; }
    }
}