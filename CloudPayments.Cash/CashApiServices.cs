using Microsoft.Extensions.DependencyInjection;

namespace CloudPayments.Cash
{
    public static class CashApiServices
    {
        public static void AddCloudPaymentCash(this IServiceCollection services, CashSettings settings)
        {
            CashApi.SetupContainer(services, settings);
        }
    }
}