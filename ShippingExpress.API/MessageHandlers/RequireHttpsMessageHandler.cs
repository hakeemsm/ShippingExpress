using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingExpress.API.MessageHandlers
{
    public class RequireHttpsMessageHandler:DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme!=Uri.UriSchemeHttps)
            {
                var responseMessage = request.CreateResponse(HttpStatusCode.Forbidden);
                responseMessage.ReasonPhrase = "Request not sent on SSL";
                return Task.FromResult(responseMessage);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
