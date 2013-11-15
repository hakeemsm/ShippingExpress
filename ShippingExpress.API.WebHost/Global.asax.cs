using System;
using System.Web.Http;
using ShippingExpress.API.Config;
using ShippingExpress.API.WebHost.App_Start;

namespace ShippingExpress.API.WebHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var configuration = GlobalConfiguration.Configuration;
            RouteConfig.RegisterRoutes(configuration);
            WebAPIConfig.Configure(configuration);
            IoCConfig.Initialize(configuration);
            EFConfig.Initialize();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}