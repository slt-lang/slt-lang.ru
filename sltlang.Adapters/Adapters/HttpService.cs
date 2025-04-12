using sltlang.Common.TelegramService;
using sltlang.Common.TelegramService.Models;
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
    public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService, ITelegramHttpAdapter
    {
        public async Task<TResponse?> GetJsonContent<TResponse>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.Content.Headers.ContentLength > 0)
            {
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();

                return response;
            }

            return default;
        }

        public async Task<(bool success, TResponse? response)> JsonRequest<TRequest, TResponse>(string name, string path, KeyValuePair<string, object>[] query, HttpMethod method, TRequest? body, TimeSpan? timeout = null)
        {
            var client = httpClientFactory.CreateClient(name);

            client.Timeout = timeout ?? TimeSpan.FromMilliseconds(1000);

            var request = new HttpRequestMessage(method, path + (query.Length > 0 ? "?" + string.Join("&", query.Select(q => $"{q.Key}={q.Value}")) : ""));
            if (body != null)
                request.Content = JsonContent.Create(body);

            var responseMessage = await client.SendAsync(request);

            return (responseMessage.IsSuccessStatusCode, await GetJsonContent<TResponse>(responseMessage));
        }

        public async Task<TResponse?> JsonRequest<TRequest, TResponse>(string name, string path, KeyValuePair<string, object>[] query, HttpMethod method, TRequest? body, Func<HttpResponseMessage, Task<TResponse?>> responseHandler, TimeSpan? timeout = null)
        {
            var client = httpClientFactory.CreateClient(name);

            client.Timeout = timeout ?? TimeSpan.FromMilliseconds(1000);

            var request = new HttpRequestMessage(method, path + (query.Length > 0 ? "?" + string.Join("&", query.Select(q => $"{q.Key}={q.Value}")) : ""));
            if (body != null)
                request.Content = JsonContent.Create(body);

            var responseMessage = await client.SendAsync(request);

            return await responseHandler(responseMessage);
        }

        public Task SendBodyMessage(string serviceName, string path, HttpMethod httpMethod, TelegramMessage telegramMessage)
            => JsonRequest<TelegramMessage, object>(serviceName, path, [], httpMethod, telegramMessage, TimeSpan.FromSeconds(30));
    }
}
