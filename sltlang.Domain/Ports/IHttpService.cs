namespace sltlang.Domain.Ports
{
    public interface IHttpService
    {
        public Task<(bool success, TResponse? response)> JsonRequest<TRequest, TResponse>(string name, string query, HttpMethod method, TRequest? body);
    }
}
