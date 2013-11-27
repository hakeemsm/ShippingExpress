using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing.Constraints;
using ShippingExpress.API.Dispatcher;

namespace ShippingExpress.API.Config
{
    public class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            HttpMessageHandler shipmentsPipeline = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(configuration),
                new[] {new ShipmentStatesDispatcher()});

            //routes.MapHttpRoute("ShipmentStatesRoute","shippingapi/shipments/{key}/shipmentstates")
            routes.MapHttpRoute("DefaultHttpRoute", "shippingapi/{controller}/{key}",
                defaults: new {key = RouteParameter.Optional}, constraints: new {key = new GuidRouteConstraint()});
        }
    }
}