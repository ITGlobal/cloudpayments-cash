using System.Threading;
using System.Threading.Tasks;
using CloudPayments.Cash.Contracts;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CloudPayments.Cash.Tests
{
    public class CashApiTests
    {
        private readonly ITestOutputHelper _output;

        public CashApiTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
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

        //[Fact]
        //public async Task Real_tests()
        //{
        //    var api = CashApi.GetDefault(new CashSettings
        //    {
        //        PublicId = "",
        //        ApiSecret = "",
        //        Inn = ""
        //    }, new LoggerFactory());

        //    var resp = await api.Receipt(new ReceiptContract
        //    {
        //        Type = "Income",
        //        AccountId = "1",
        //        InvoiceId = "123",
        //        CustomerReceipt = new CustomerReceipt
        //        {
        //            Email = "shergin@itglobal.ru",
        //            TaxationSystem = TaxationSystem.Common,
        //            Items = new List<CustomerReceiptItem>
        //            {
        //                new CustomerReceiptItem
        //                {
        //                    Label = "тестовый курс",
        //                    Price = 123,
        //                    Amount = 123,
        //                    Quantity = 1,
        //                    Vat = null
        //                }
        //            }
        //        }
        //    }, "123", CancellationToken.None);
        //    resp.Success.Should().BeTrue(resp.Message);
        //}
    }
}