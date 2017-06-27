using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPayments.Cash.Tests.Utils
{
    public class TestHttpHandler : IHttpHandler
    {
        private readonly Action<string> _assertRequestJson;
        private readonly object _responseObj;

        public TestHttpHandler(Action<string> assertRequestJson, object responseObj)
        {
            _assertRequestJson = assertRequestJson;
            _responseObj = responseObj;
        }

        public void SetupIdempotent(string requestId)
        {
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken token)
        {
            var json = await content.ReadAsStringAsync();
            _assertRequestJson(json);
            return new HttpResponseMessage { Content = CashHttpClient.ToJsonStreamContent(_responseObj) };
        }
    }
}