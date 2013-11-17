using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingExpress.Tests.TestHelpers
{
    public static class DelegatingHandlerExtensions
    {
        internal static Task<HttpResponseMessage> InvokeAsync(this DelegatingHandler handler, HttpRequestMessage request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            handler.InnerHandler = new DummyHandler();
            var invoker = new HttpMessageInvoker(handler);
            return invoker.SendAsync(request, cancellationToken);
        }
    }

    internal class DummyHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
