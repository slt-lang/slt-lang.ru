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
        public async Task<(bool success, TResponse? response)> JsonRequest<TRequest, TResponse>(string name, string path, KeyValuePair<string, object>[] query, HttpMethod method, TRequest? body, TimeSpan? timeout = null)
        {
            var client = httpClientFactory.CreateClient(name);

            client.Timeout = timeout ?? TimeSpan.FromMilliseconds(1000);

            var request = new HttpRequestMessage(method, path + (query.Length > 0 ? "?" + string.Join("&", query.Select(q => $"{q.Key}={q.Value}")) : ""));
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
