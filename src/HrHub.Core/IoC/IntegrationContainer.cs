using HrHub.Abstraction.Factories;
using HrHub.Abstraction.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
namespace HrHub.Core.IoC
{
    public static class IntegrationContainer
    {
        public static void RegisterIntegration(this IServiceCollection services, Action<HttpClientConfiguration> configuration)
        {
            services.AddScoped<IHttpClientHelperFactory, HttpClientHelperFactory>();

            var integrationConfiguration = new HttpClientConfiguration();
            configuration(integrationConfiguration);

            foreach (var client in integrationConfiguration.HttpClients)
            {
                services.AddHttpClient<IHttpClientHelperFactory, HttpClientHelperFactory>(client.Name, options =>
                {
                    options.BaseAddress = new Uri(client.BaseUrl);
                    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });
            }
        }
    }
}
