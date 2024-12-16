namespace HrHub.Application.Helpers
{
    public static class IntegrationHelper
    {
        private static IHttpClientFactory _httpClientFactory;

        public static void IntegrationHelperConfigure(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public static HttpClient CreateHttpClient(string clientName)
        {
            return _httpClientFactory.CreateClient(clientName);
        }

        public static string CreateQueryString<TRequest>(TRequest request) where TRequest : class
        {
            var properties = typeof(TRequest).GetProperties()
            .Where(prop => prop.GetValue(request) != null) // Nullable olmayanları ve null olmayanları al
            .Select(prop => $"{prop.Name}={Uri.EscapeDataString(prop.GetValue(request).ToString())}");

            return string.Join("&", properties);
        }
    }
}
