using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CloudPayments.Cash
{
    internal class HttpHandler : IHttpHandler
    {
        private readonly HttpClient _client;
        private readonly ILogger<IHttpHandler> _logger;

        public HttpHandler(CashSettings settings, ILogger<IHttpHandler> logger)
        {
            _client = new HttpClient { BaseAddress = new Uri(settings.Endpoint) };
            _logger = logger;

            var credentials = Encoding.ASCII.GetBytes($"{settings.PublicId}:{settings.ApiSecret}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
        }

        /// <summary>
        /// Make requests idempotent
        /// </summary>
        /// <param name="requestId"></param>
        public void SetupIdempotent(string requestId)
        {
            if (requestId != null)
            {
                _logger.LogTrace($"request will be idempotent with X-Request-ID = {requestId}");

                _client.DefaultRequestHeaders.Remove("X-Request-ID");
                _client.DefaultRequestHeaders.Add("X-Request-ID", requestId);
            }
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken token)
        {
            return _client.PostAsync(url, content, token);
        }
    }
}