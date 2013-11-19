using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingExpress.Tests.TestHelpers
{
    public class TestHelper
    {
        public static Task<HttpResponseMessage> InvokeMessageHandler(HttpRequestMessage requestMessage, DelegatingHandler handler, CancellationToken cancellationToken=default(CancellationToken))
        {
            handler.InnerHandler=new DummyHandler();
            var messageInvoker = new HttpMessageInvoker(handler);
            return messageInvoker.SendAsync(requestMessage, cancellationToken);
        }
    }
}