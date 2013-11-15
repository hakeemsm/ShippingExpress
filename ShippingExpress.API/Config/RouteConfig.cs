using System.Web.Http;
using System.Web.Http.Routing.Constraints;

namespace ShippingExpress.API.Config
{
    public class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("DefaultHttpRoute", "shippingapi/{controller}/{key}",
                defaults: new {key = RouteParameter.Optional}, constraints: new {key = new GuidRouteConstraint()});
        }
    }
}