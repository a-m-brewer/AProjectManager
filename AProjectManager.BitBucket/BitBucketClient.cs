using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AProjectManager.Domain.BitBucket;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AProjectManager.BitBucket
{
    public class BitBucketClient : IBitBucketClient
    {
        private const string AuthUrl = "https://bitbucket.org/site/oauth2/access_token";
        
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        public async Task<TokenDto> Authorize(AuthorizationCredentials authorizationCredentials)
        {
            var client = CreateHttpClient(authorizationCredentials);

            var timeBeforeRequest = DateTime.UtcNow;
            
            var response = await client.PostAsync(AuthUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            }));

            return await ToToken(response.Content, timeBeforeRequest);
        }

        public async Task<TokenDto> Authorize(RefreshTokenRequest refreshTokenRequest)
        {
            var client = CreateHttpClient(refreshTokenRequest.AuthorizationCredentials);

            var timeBeforeRequest = DateTime.UtcNow;
            
            var response = await client.PostAsync(AuthUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshTokenRequest.RefreshToken),
            }));

            return await ToToken(response.Content, timeBeforeRequest);
        }

        private HttpClient CreateHttpClient(AuthorizationCredentials authorizationCredentials)
        {
            return new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization = GetBasicAuth(authorizationCredentials)
                }
            };
        }

        private async Task<TokenDto> ToToken(HttpContent content, DateTime timeBeforeRequest)
        {
            var responseContentString = await content.ReadAsStringAsync();
            var original = JsonConvert.DeserializeObject<Token>(responseContentString, _jsonSerializerSettings);
            return new TokenDto
            {
                Token = original,
                TimeAuthenticated = timeBeforeRequest
            };
        }

        private AuthenticationHeaderValue GetBasicAuth(AuthorizationCredentials authorizationCredentials)
        {
            return new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{authorizationCredentials.Key}:{authorizationCredentials.Secret}")));
        }
    }
}