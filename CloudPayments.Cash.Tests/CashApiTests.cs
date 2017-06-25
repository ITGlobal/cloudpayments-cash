using System.Threading;
using System.Threading.Tasks;
using CloudPayments.Cash.Contracts;
using Moq;
using Xunit;

namespace CloudPayments.Cash.Tests
{
    public class CashApiTests
    {
        [Fact]
        public async Task Test_test_settings()
        {
            var client = new Mock<ICashHttpClient>();
            client.Setup(_ => _.PostAsync<ReceiptContract, CashResponse>(It.IsAny<string>(), It.IsAny<ReceiptContract>(), It.IsAny<string>(),
                CancellationToken.None)).ReturnsAsync(new CashResponse { Success = true });
            
            var cashapi = new CashApi(client.Object, new CashSettings { Test = true });
            await cashapi.Receipt(new ReceiptContract {Inn = "123"}, token: CancellationToken.None);

            client.Verify(_ => _.PostAsync<ReceiptContract, CashResponse>("/test", It.IsAny<ReceiptContract>(), null,
                CancellationToken.None), Times.Once);

            cashapi = new CashApi(client.Object, new CashSettings { Test = false });
            await cashapi.Receipt(new ReceiptContract { Inn = "123" }, token: CancellationToken.None);

            client.Verify(_ => _.PostAsync<ReceiptContract, CashResponse>("/kkt/receipt", It.IsAny<ReceiptContract>(), null,
                CancellationToken.None), Times.Once);
        }
    }
}