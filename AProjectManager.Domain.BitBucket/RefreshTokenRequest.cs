namespace AProjectManager.Domain.BitBucket
{
    public class RefreshTokenRequest
    {
        public AuthorizationCredentials AuthorizationCredentials { get; set; }
        public string RefreshToken { get; set; }
    }
}