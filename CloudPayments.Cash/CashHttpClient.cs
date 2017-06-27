using System.IO;
using System.Net.Http;
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

            using (var stringContent = await ToJsonStringContent(request))
            using (var response = await _client.PostAsync(url, stringContent, token))
            using (var content = response.Content)
            {
                var result = FromJsonStream<TResponse>(await content.ReadAsStreamAsync());
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
        public static Stream ToJsonStream<TObject>(TObject obj)
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
        public static TObject FromJsonStream<TObject>(Stream stream) where TObject : class
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
        public static StreamContent ToJsonStreamContent<TObject>(TObject obj)
        {
            return new StreamContent(ToJsonStream(obj));
        }

        /// <summary>
        /// Make json string from obj
        /// </summary>
        /// <typeparam name="TObject">type of object</typeparam>
        /// <param name="obj">object</param>
        /// <returns>json string</returns>
        public static async Task<string> ToJson<TObject>(TObject obj)
        {
            return await ToJsonStreamContent(obj).ReadAsStringAsync();
        }

        /// <summary>
        /// Make json StringContent from obj
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <returns>StringContent object</returns>
        public static async Task<StringContent> ToJsonStringContent<TObject>(TObject obj)
        {
            return new StringContent(await ToJson(obj), Encoding.UTF8, "application/json");
        }

        #endregion
    }
}