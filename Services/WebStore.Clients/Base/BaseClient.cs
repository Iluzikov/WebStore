using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
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

    }
}
