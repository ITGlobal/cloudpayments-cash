using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPayments.Cash
{
    public interface IHttpHandler
    {
        /// <summary>
        /// Make requests idempotent
        /// </summary>
        /// <param name="requestId"></param>
        void SetupIdempotent(string requestId);
        
        Task<HttpResponseMessage> PostAsync(string url, StreamContent toJsonsStreamContent, CancellationToken token);
    }
}