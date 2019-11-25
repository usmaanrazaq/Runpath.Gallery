using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Runpath.Gallery.Service.External
{
    public class HttpClientCaller<T> : IHttpClientCaller<T>
    {
        private readonly HttpClient _client;
        public HttpClientCaller(HttpClient client)
        {
            _client = client;
        }

        public async Task<ICollection<T>> GetApiCollection(string url)
        {
            var response = await _client.GetAsync(url);

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<ICollection<T>>(content));
        }


        public async Task<T> GetApi(string url)
        {
            var response = await _client.GetAsync(url);
            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(content));
        }

    }
}
