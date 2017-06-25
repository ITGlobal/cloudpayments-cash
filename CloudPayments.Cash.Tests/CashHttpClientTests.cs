using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
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
            }), logger.Object);

            var result = await client.PostAsync<TestRequest, TestResponse>("/some/api", new TestRequest { TestEnum = TestEnum.First, Test = "asdfasdf" });
            result.Success.Should().BeTrue();
        }

        [Fact]
        public void ToJsonsStream_FromJsonStram_Tests()
        {
            var stream = CashHttpClient.ToJsonsStream(new TestRequest {TestEnum = TestEnum.Second, Test = "asdfasdf"});
            var obj = CashHttpClient.FromJsonStram<TestRequest>(stream);
            obj.TestEnum.Should().Be(TestEnum.Second);
            obj.Test.Should().Be("asdfasdf");
        }

        class TestHttpHandler : IHttpHandler
        {
            private readonly Action<string> _assertRequestJson;

            public TestHttpHandler(Action<string> assertRequestJson)
            {
                _assertRequestJson = assertRequestJson;
            }
            
            public void SetupIdempotent(string requestId)
            {
            }

            public async Task<HttpResponseMessage> PostAsync(string url, StreamContent toJsonsStreamContent, CancellationToken token)
            {
                var json = await toJsonsStreamContent.ReadAsStringAsync();
                _assertRequestJson(json);
                return new HttpResponseMessage { Content = CashHttpClient.ToJsonsStreamContent(new TestResponse { Success = true}) };
            }
        }

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
