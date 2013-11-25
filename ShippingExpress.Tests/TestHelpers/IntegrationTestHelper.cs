using System.Web.Http;
using ShippingExpress.API.Config;
using StructureMap;

namespace ShippingExpress.Tests.TestHelpers
{
    public class IntegrationTestHelper
    {
        public static HttpConfiguration GetInitialIntegrationTestConfig(IContainer container)
        {
            HttpConfiguration config = GetInitialIntegrationTestConfig();
            IoCConfig.Initialize(config);
            return config;
        }

        private static HttpConfiguration GetInitialIntegrationTestConfig()
        {
            var configuration = new HttpConfiguration();
            RouteConfig.RegisterRoutes(configuration);
            WebAPIConfig.Configure(configuration);
            return configuration;
        }
    }
}
