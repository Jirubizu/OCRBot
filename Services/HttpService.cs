using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OCRBot.Services
{
    public class HttpService : HttpClient
    {
        private const string ProxyUrl = "https://proxy.duckduckgo.com/iu/?u=";

        public HttpService()
        {
            this.Timeout = new TimeSpan(0, 0, 10);
            this.MaxResponseContentBufferSize = 8000000; // Limit for non-nitro users. (8mb)
        }

        public async Task<JObject> GetJObjectAsync(string url)
        {
            try
            {
                return JObject.Parse(await this.GetStringAsync(url));
            }
            catch
            {
                return null;
            }
        }

        public async Task<JArray> GetJArrayAsync(string url)
        {
            try
            {
                return JArray.Parse(await GetStringAsync(url));
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> GetJsonAsync<T>(string url)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await GetStringAsync(url));
            }
            catch
            {
                return default(T);
            }
        }
    }
}