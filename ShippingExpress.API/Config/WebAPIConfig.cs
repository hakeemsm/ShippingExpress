using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;

namespace ShippingExpress.API.Config
{
    public class WebAPIConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            var jqueryFormatter = configuration.Formatters.FirstOrDefault(x => x.GetType() == typeof (JQueryMvcFormUrlEncodedFormatter));
            configuration.Formatters.Remove(jqueryFormatter);
            configuration.Formatters.Remove(configuration.Formatters.FormUrlEncodedFormatter);
            foreach (var formatter in configuration.Formatters)
            {
                formatter.RequiredMemberSelector = new SuppressedRequiredMemberSelector();
            }
            configuration.Services.Replace(typeof(IContentNegotiator),new DefaultContentNegotiator(true));
            configuration.Services.RemoveAll(typeof (ModelValidatorProvider),
                v => !(v is DataAnnotationsModelValidatorProvider));

        }
    }

    public class SuppressedRequiredMemberSelector : IRequiredMemberSelector
    {
        public bool IsRequiredMember(MemberInfo member)
        {
            return false;
        }
    }
}