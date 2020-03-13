using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AProjectManager.BitBucket
{
    public static class HttpClientExtensions
    {
        public static async Task<T> DeserializeContent<T>(this HttpResponseMessage httpResponseMessage, JsonSerializerSettings jsonSerializerSettings)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content, jsonSerializerSettings);
        }
    }
}