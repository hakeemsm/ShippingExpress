using System.Net.Http;
using System.Web.Http.Dependencies;

namespace ShippingExpress.API.HttpExtensions
{
    public static class HttpRequestMessageExtensions
    {
        public static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            return (TService) dependencyScope.GetService(typeof (TService));
        }
    }
}
