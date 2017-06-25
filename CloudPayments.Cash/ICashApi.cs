using System.Threading;
using System.Threading.Tasks;
using CloudPayments.Cash.Contracts;

namespace CloudPayments.Cash
{
    public interface ICashApi
    {
        /// <summary>
        /// Make cash voucher in CloudPayment
        /// </summary>
        /// <param name="contract">voucher contract</param>
        /// <param name="requestId">[optional] param for idempotent request</param>
        /// <param name="token">cancellation token</param>
        Task<CashResponse> Receipt(ReceiptContract contract, string requestId = null,
            CancellationToken token = default(CancellationToken));
    }
}