using System.Threading;
using System.Threading.Tasks;
using CloudPayments.Cash.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudPayments.Cash
{
    public class CashApi: ICashApi
    {
        private readonly ICashHttpClient _cashHttpClient;
        private readonly CashSettings _settings;

        public CashApi(ICashHttpClient cashHttpClient, CashSettings settings)
        {
            _cashHttpClient = cashHttpClient;
            _settings = settings;
        }

        /// <summary>
        /// Make cash voucher in CloudPayment
        /// </summary>
        /// <param name="contract">voucher contract</param>
        /// <param name="requestId">[optional] param for idempotent request</param>
        /// <param name="token">cancellation token</param>
        public async Task<CashResponse> Receipt(ReceiptContract contract, string requestId = null, CancellationToken token = default(CancellationToken))
        {
            var url = _settings.Test ? "/test" : "/kkt/receipt";
            return await _cashHttpClient.PostAsync<ReceiptContract, CashResponse>(url, contract, requestId, token);
        }

        #region Static

        /// <summary>
        /// setup default api implementation with container
        /// </summary>
        /// <param name="services">services description</param>
        /// <param name="settings">settings for cash api</param>
        public static void SetupContainer(IServiceCollection services, CashSettings settings)
        {
            services.AddSingleton(settings);
            //services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
            
            services.AddTransient<IHttpHandler, HttpHandler>();
            services.AddTransient<ICashHttpClient, CashHttpClient>();
            services.AddTransient<ICashApi, CashApi>();
        }

        /// <summary>
        /// get default implementation of CashApi
        /// </summary>
        /// <param name="settings">settings for cash api</param>
        /// <param name="loggerFactory">logger factory</param>
        /// <returns>api interface</returns>
        public static ICashApi GetDefault(CashSettings settings, ILoggerFactory loggerFactory)
        {
            return new CashApi(
                new CashHttpClient(
                    new HttpHandler(settings, loggerFactory.CreateLogger<IHttpHandler>()), 
                    loggerFactory.CreateLogger<ICashHttpClient>()
                ),
                settings
            );
        }

        #endregion
    }
}