using System.Runtime.Serialization;
using System.Threading.Tasks;
using CloudPayments.Cash.Tests.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CloudPayments.Cash.Tests
{
    public class CashHttpClientTests
    {
        [Fact]
        public async Task Test_PostAsync_json_serialization()
        {
            var logger = new Mock<ILogger<ICashHttpClient>>(MockBehavior.Default);
            var client = new CashHttpClient(new TestHttpHandler((json) =>
            {
                json.Should().Be("{\"test\":\"asdfasdf\",\"testenum\":1}");
            }, new TestResponse { Success = true }), logger.Object);

            var result = await client.PostAsync<TestRequest, TestResponse>("/some/api", new TestRequest { TestEnum = TestEnum.First, Test = "asdfasdf" });
            result.Success.Should().BeTrue();
        }

        [Fact]
        public void ToJsonsStream_FromJsonStram_Tests()
        {
            var stream = CashHttpClient.ToJsonStream(new TestRequest {TestEnum = TestEnum.Second, Test = "asdfasdf"});
            var obj = CashHttpClient.FromJsonStream<TestRequest>(stream);
            obj.TestEnum.Should().Be(TestEnum.Second);
            obj.Test.Should().Be("asdfasdf");
        }


        [KnownType(typeof(TestResponse))]
        public class TestResponse
        {
            public bool Success { get; set; }
        }

        [DataContract]
        public class TestRequest
        {
            [DataMember(Name = "testenum")]
            public TestEnum TestEnum { get; set; }

            [DataMember(Name = "test")]
            public string Test { get; set; }
        }

        public enum TestEnum
        {
            First = 1,
            Second = 2
        }

    }

}
