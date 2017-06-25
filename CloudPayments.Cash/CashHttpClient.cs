using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CloudPayments.Cash
{
    /// <summary>
    /// Http client for CloudPayment Cash Api
    /// </summary>
    public class CashHttpClient : ICashHttpClient
    {
        private readonly IHttpHandler _client;
        private readonly ILogger<ICashHttpClient> _logger;

        public CashHttpClient(IHttpHandler httpHandler, ILogger<ICashHttpClient> logger)
        {
            _client = httpHandler;
            _logger = logger;
        }

        /// <summary>
        /// Method convert request object to JSON, make POST request and convert response to JSON
        /// </summary>
        /// <typeparam name="TRequest">type of request json object</typeparam>
        /// <typeparam name="TResponse">type of response json object</typeparam>
        /// <param name="url">part of url like "/kkt/receipt"</param>
        /// <param name="request">request contract object</param>
        /// <param name="requestId">parameter for idempotent requests</param>
        /// <param name="token">cancellation token</param>
        /// <returns>response contract object</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, string requestId = null, CancellationToken token = default(CancellationToken)) where TResponse : class
        {
            _logger.LogTrace($"make request to {url}");
            _client.SetupIdempotent(requestId);

            using (var requestStreamContent = ToJsonsStreamContent(request))
            using (var response = await _client.PostAsync(url, requestStreamContent, token))
            using (var content = response.Content)
            {
                var result = FromJsonStram<TResponse>(await content.ReadAsStreamAsync());
                _logger.LogTrace($"response successfully received");
                return result;
            }
        }

        #region Helpers


        /// <summary>
        /// Convert object to memory stream with json data
        /// </summary>
        /// <typeparam name="TObject">Type of object</typeparam>
        /// <param name="obj">object</param>
        /// <returns>memory stream with json data</returns>
        public static Stream ToJsonsStream<TObject>(TObject obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(TObject));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, obj);

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Convert stream with json data to C# object
        /// </summary>
        /// <typeparam name="TObject">Type of object</typeparam>
        /// <param name="stream">stream with json data</param>
        /// <returns>object with json data</returns>
        public static TObject FromJsonStram<TObject>(Stream stream) where TObject : class
        {
            var serializer = new DataContractJsonSerializer(typeof(TObject));
            var json = serializer.ReadObject(stream) as TObject;
            return json;
        }

        /// <summary>
        /// Make StreamContent from obj
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static StreamContent ToJsonsStreamContent<TObject>(TObject obj)
        {
            return new StreamContent(ToJsonsStream(obj));
        }

        #endregion
    }
}