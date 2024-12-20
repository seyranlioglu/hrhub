using HrHub.Abstraction.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HrHub.Abstraction.Factories
{
    public class HttpClientHelperFactory : IHttpClientHelperFactory
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelperFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null)
        {

            if (certificateIgnore)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            string content = JsonConvert.SerializeObject(parameters);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (extraHeaders != null)
            {
                foreach (var item in extraHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                if (extraHeaders.ContainsKey("Timeout"))
                {
                    _httpClient.Timeout = TimeSpan.Parse(extraHeaders["Timeout"]);
                }
            }

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }

            var response = await _httpClient.PostAsync(method, byteContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var resultErr = await response.Content.ReadAsStringAsync();
                var err = JsonConvert.DeserializeObject<ErrModel>(resultErr);
                throw new BusinessException((int)HttpStatusCode.BadRequest, err.Message);
            }

            var result = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<TResponse>(result);
            return obj;

        }
        public async Task<TResponse> GetAsync<TResponse>(string method, string token = null, Dictionary<string, string> extraHeaders = null, TimeSpan? timeout = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (extraHeaders != null)
            {
                foreach (var item in extraHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                if (extraHeaders.ContainsKey("Timeout"))
                {
                    _httpClient.Timeout = TimeSpan.Parse(extraHeaders["Timeout"]);
                }
            }

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }

            var response = await _httpClient.GetAsync(method).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var resultErr = await response.Content.ReadAsStringAsync();
                var err = JsonConvert.DeserializeObject<ErrModel>(resultErr);
                throw new BusinessException((int)HttpStatusCode.BadRequest, err.Message);
            }
            string result = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<TResponse>(result);

            return obj;
        }
        public async Task<TResponse> PutAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null)
        {
            if (certificateIgnore)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            string content = JsonConvert.SerializeObject(parameters);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (extraHeaders != null)
            {
                foreach (var item in extraHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                if (extraHeaders.ContainsKey("Timeout"))
                {
                    _httpClient.Timeout = TimeSpan.Parse(extraHeaders["Timeout"]);
                }
            }

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }

            var response = await _httpClient.PutAsync(method, byteContent).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var resultErr = await response.Content.ReadAsStringAsync();
                var err = JsonConvert.DeserializeObject<ErrModel>(resultErr);
                throw new BusinessException((int)HttpStatusCode.BadRequest, err.Message);
            }
            string result = await response.Content.ReadAsStringAsync();



            try
            {
                var obj = JsonConvert.DeserializeObject<TResponse>(result);
                return obj;
            }
            catch (Exception exp)
            {
                var message = exp.InnerException + " ;RequestUrl : " + _httpClient.BaseAddress + method + " ;Put Url : " + response.RequestMessage + " ;Request Body : " + content + " ;Result Json : " + result;
                throw new BusinessException(message);
            }
        }
        public async Task<TResponse> PatchAsync<TResponse, TRequest>(string method, TRequest parameters, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null)
        {
            if (certificateIgnore)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            string content = JsonConvert.SerializeObject(parameters);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (extraHeaders != null)
            {
                foreach (var item in extraHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                if (extraHeaders.ContainsKey("Timeout"))
                {
                    _httpClient.Timeout = TimeSpan.Parse(extraHeaders["Timeout"]);
                }
            }

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), method) { Content = byteContent };
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var resultErr = await response.Content.ReadAsStringAsync();
                var err = JsonConvert.DeserializeObject<ErrModel>(resultErr);
                throw new BusinessException((int)HttpStatusCode.BadRequest, err.Message);
            }

            string result = await response.Content.ReadAsStringAsync();
            try
            {
                var obj = JsonConvert.DeserializeObject<TResponse>(result);
                return obj;
            }
            catch (Exception exp)
            {
                var message = exp.InnerException + " ;RequestUrl : " + _httpClient.BaseAddress + method + " ;Patch Url : " + response.RequestMessage + " ;Request Body : " + content + " ;Result Json : " + result;
                throw new BusinessException(message);
            }
        }
        public async Task<TResponse> DeleteAsync<TResponse>(string method, string token = null, Dictionary<string, string> extraHeaders = null, bool certificateIgnore = false, TimeSpan? timeout = null)
        {
            if (certificateIgnore)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (extraHeaders != null)
            {
                foreach (var item in extraHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                if (extraHeaders.ContainsKey("Timeout"))
                {
                    _httpClient.Timeout = TimeSpan.Parse(extraHeaders["Timeout"]);
                }
            }
            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }
            var response = await _httpClient.DeleteAsync(method).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var resultErr = await response.Content.ReadAsStringAsync();
                var err = JsonConvert.DeserializeObject<ErrModel>(resultErr);
                throw new BusinessException((int)HttpStatusCode.BadRequest, err.Message);
            }
            string result = await response.Content.ReadAsStringAsync();
            try
            {
                var obj = JsonConvert.DeserializeObject<TResponse>(result);
                return obj;
            }
            catch (Exception exp)
            {
                var message = exp.InnerException + " ;RequestUrl : " + _httpClient.BaseAddress + method + " ;Delete Url : " + response.RequestMessage + " ;Result Json : " + result;
                throw new BusinessException(message);
            }
        }
    }
    public class ErrModel
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}

