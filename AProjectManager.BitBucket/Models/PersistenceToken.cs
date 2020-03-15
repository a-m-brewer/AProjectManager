using System;

namespace AProjectManager.BitBucket.Models
{
    public class PersistenceToken
    {
        public Token Token { get; set; }
        public DateTime TimeAuthenticated { get; set; }
    }
}