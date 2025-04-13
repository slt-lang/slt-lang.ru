namespace sltlang.Domain.Ports
{
    public interface IHttpService
    {
        public Task<TResponse?> GetJsonContent<TResponse>(HttpResponseMessage httpResponseMessage);
        public Task<(bool success, TResponse? response)> JsonRequest<TRequest, TResponse>(string name, string path, KeyValuePair<string, object>[] query, HttpMethod method, TRequest? body, TimeSpan? timeout = null);
        public Task<TResponse?> JsonRequest<TRequest, TResponse>(string name, string path, KeyValuePair<string, object>[] query, HttpMethod method, TRequest? body, Func<HttpResponseMessage, Task<TResponse?>> responseHandler, TimeSpan? timeout = null);
    }
}
