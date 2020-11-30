using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly string _serviceAddress;
        protected readonly HttpClient _client;

        public BaseClient(IConfiguration configuration, string serviceAddress)
        {
            _serviceAddress = serviceAddress;
            _client = new HttpClient
            {
                BaseAddress = new Uri(configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")}
                }
            };
        }

        //Get
        public T Get<T>(string url) => GetAsync<T>(url).Result;
        public async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default)
        {
            var response = await _client.GetAsync(url, Cancel);
            return await response.EnsureSuccessStatusCode().Content.ReadAsAsync<T>(Cancel);
        }

        //Post
        public HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _client.PostAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        //Put
        public HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _client.PutAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        //Delete
        public HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        public async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            var response = await _client.DeleteAsync(url, Cancel);
            return response;
        }


        #region Dispose

        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
        }

        #endregion

    }
}
