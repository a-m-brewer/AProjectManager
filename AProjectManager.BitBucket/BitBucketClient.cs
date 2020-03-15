using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AProjectManager.BitBucket.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AProjectManager.BitBucket
{
    public class BitBucketClient : IBitBucketClient
    {
        private const string AuthUrl = "https://bitbucket.org/site/oauth2/access_token";
        private const string BaseUrl = "https://bitbucket.org/!api/2.0";
        public const int PageLength = 100;
        
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        public async Task<TokenDto> Authorize(AuthorizationCredentials authorizationCredentials, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient(authorizationCredentials);

            var timeBeforeRequest = DateTime.UtcNow;
            
            var response = await client.PostAsync(AuthUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            }), cancellationToken);

            return await ToToken(response, timeBeforeRequest);
        }

        public async Task<TokenDto> AuthorizeAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient(refreshTokenRequest.AuthorizationCredentials);

            var timeBeforeRequest = DateTime.UtcNow;
            
            var response = await client.PostAsync(AuthUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshTokenRequest.RefreshToken),
            }), cancellationToken);

            return await ToToken(response, timeBeforeRequest);
        }

        public async Task<List<ApiRepo>> GetRepositoriesAsync(GetRepositoriesRequest getRepositoriesRequest, CancellationToken cancellationToken = default)
        {
            var apiAddress = $"{BaseUrl}/repositories/{getRepositoriesRequest.User}";

            var queryParameters = GetRepositoriesRequest.ToNamedValueCollection(getRepositoriesRequest);

            var fullApiAddress = BuildUri(apiAddress, queryParameters);
            
            using var client = new HttpClient();

            var nextPageToken = "startValue";

            var allContent = new List<ApiRepo>();
            
            while (nextPageToken != null)
            {
                var response = await client.GetAsync(fullApiAddress, cancellationToken);
                var content = await response.DeserializeContent<GetRepositoriesModel>(_jsonSerializerSettings);
                allContent.AddRange(content.Values);

                nextPageToken = content.Next;
            }

            return allContent;
        }

        private static Uri BuildUri(string baseUrl, NameValueCollection queryParameters)
        {
            var builder = new UriBuilder(baseUrl) {Port = -1};
            var query = HttpUtility.ParseQueryString(builder.Query);
            query.Add(queryParameters);
            builder.Query = query.ToString();
            return builder.Uri;
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

        private async Task<TokenDto> ToToken(HttpResponseMessage content, DateTime timeBeforeRequest)
        {
            var original = await content.DeserializeContent<Token>(_jsonSerializerSettings);
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