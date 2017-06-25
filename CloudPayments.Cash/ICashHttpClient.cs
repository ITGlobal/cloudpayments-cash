using System.Threading;
using System.Threading.Tasks;

namespace CloudPayments.Cash
{
    /// <summary>
    /// Http client for CloudPayment Cash Api
    /// </summary>
    public interface ICashHttpClient
    {
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
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, string requestId = null,
            CancellationToken token = default(CancellationToken)) where TResponse : class;
    }
}