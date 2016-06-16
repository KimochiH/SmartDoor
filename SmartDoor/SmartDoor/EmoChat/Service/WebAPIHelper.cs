using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmoChat.Service
{
    public class WebAPIHelper
    {
        const string APPLICATION_JSON = "application/json";


        readonly HttpClient _httpClient;
        CancellationTokenSource _cts;

        public string AccessToken { get; set; }

        public WebAPIHelper()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
            };

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            _httpClient = new HttpClient(handler);

        }

        private HttpRequestMessage RequestMessage(HttpMethod method, string url)
        {
            HttpRequestMessage t = new HttpRequestMessage(method, url);
            t.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            t.Headers.TryAddWithoutValidation("Accept", "*/*");
            t.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            if (!string.IsNullOrEmpty(AccessToken))
            {
                t.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            return t;
        }
        private HttpRequestMessage RequestMessage(HttpMethod method, Uri uri)
        {
            HttpRequestMessage t = new HttpRequestMessage(method, uri);
            //t.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            //t.Headers.TryAddWithoutValidation("Accept", "*/*");
            //t.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            if (!string.IsNullOrEmpty(AccessToken))
            {
                t.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            return t;
        }
        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        public async Task<string> GetAsync(Uri path)
        {
            var response = await _httpClient.SendAsync(RequestMessage(HttpMethod.Get, path));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }

        public async Task<string> PutAsync<T>(Uri path, T payload)
        {
            var data = Serialize(payload);
            var request = RequestMessage(HttpMethod.Put, path);
            request.Content = new StringContent(data, Encoding.UTF8, APPLICATION_JSON);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;

        }

        public Task<string> PostAsync(Uri path, object payload)
        {
            return PostAsync(path, payload, APPLICATION_JSON);
        }

        public async Task<string> PostAsync(Uri path, object payload, string mediaType)
        {
            var data = Serialize(payload);
            var request = RequestMessage(HttpMethod.Post, path);
            request.Content = new StringContent(data, Encoding.UTF8, mediaType);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }

        public async Task<string> PostAsync(Uri path)
        {
            var data = "";
            var request = RequestMessage(HttpMethod.Post, path);
            request.Content = new StringContent(data, Encoding.UTF8, APPLICATION_JSON);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }

        public async Task<string> PostAsync(Uri path, Dictionary<string, object> payload)
        {

            var keyValues = new List<KeyValuePair<string, string>>();
            foreach (var o in payload)
                keyValues.Add(new KeyValuePair<string, string>(o.Key, o.Value.ToString()));

            var form = new FormUrlEncodedContent(keyValues);

            var response = await _httpClient.PostAsync(path, form);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }

        public async Task<string> DeleteAsync(Uri path)
        {
            var response = await _httpClient.SendAsync(RequestMessage(HttpMethod.Delete, path));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}