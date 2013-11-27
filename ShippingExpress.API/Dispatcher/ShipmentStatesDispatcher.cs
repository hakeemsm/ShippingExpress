using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShippingExpress.API.HttpExtensions;
using ShippingExpress.Domain.Services;

namespace ShippingExpress.API.Dispatcher
{
    public class ShipmentStatesDispatcher:DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var routeData = request.GetRouteData();
            Guid shipmentKey = Guid.ParseExact(routeData.Values["key"].ToString(), "D");
            var shipmentService = request.GetService<IShipmentService>();
            if (shipmentService.GetShipment(shipmentKey) != null)
                return Task.FromResult(request.CreateResponse(HttpStatusCode.NotFound));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
