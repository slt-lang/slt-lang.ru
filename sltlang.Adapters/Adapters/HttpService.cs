using sltlang.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Adapters.Adapters
{
    public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
    {
        public async Task<(bool success, TResponse? response)> JsonRequest<TRequest, TResponse>(string name, string query, HttpMethod method, TRequest? body)
        {
            var client = httpClientFactory.CreateClient(name);

            var request = new HttpRequestMessage(method, query);
            if (body != null)
                request.Content = JsonContent.Create(body);

            var responseMessage = await client.SendAsync(request);

            if (responseMessage.Content.Headers.ContentLength > 0)
            {
                var response = await responseMessage.Content.ReadFromJsonAsync<TResponse>();

                return (responseMessage.IsSuccessStatusCode, response);
            }

            return (responseMessage.IsSuccessStatusCode, default);
        }
    }
}
