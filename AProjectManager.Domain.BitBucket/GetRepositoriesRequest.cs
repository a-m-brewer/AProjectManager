using System;
using System.Collections.Specialized;

namespace AProjectManager.Domain.BitBucket
{
    public class GetRepositoriesRequest
    {
        public string User { get; }
        public string AccessToken { get; }

        public GetRepositoriesRequest(string user, string accessToken)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        public string Role { get; set; } = "member";
        public string ProjectKey { get; set; } = default;

        public int PageLen { get; set; } = 100;

        public static NameValueCollection ToNamedValueCollection(GetRepositoriesRequest getRepositoriesRequest)
        {
            var queryParameters = new NameValueCollection
            {
                {"access_token", getRepositoriesRequest.AccessToken},
                {"role", getRepositoriesRequest.Role},
                {"pagelen", getRepositoriesRequest.PageLen.ToString()},
            };
            
            if (getRepositoriesRequest.ProjectKey != default)
                queryParameters.Add("q", $"project.key=\"{getRepositoriesRequest.ProjectKey}\"");

            return queryParameters;
        }
    }
}