namespace HrHub.Abstraction.Factories
{
    public interface IHttpClientHelperFactory
    {
        Task<TResponse> PostAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null);
        Task<TResponse> GetAsync<TResponse>(string method, string token = null, Dictionary<string, string> extraHeaders = null, TimeSpan? timeout = null);
        Task<TResponse> PatchAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null);
        Task<TResponse> PutAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null);
        Task<TResponse> DeleteAsync<TResponse>(string method, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null);

    }
}
