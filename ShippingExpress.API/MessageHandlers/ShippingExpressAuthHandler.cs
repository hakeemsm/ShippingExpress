using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using ShippingExpress.Domain.Services;

namespace ShippingExpress.API.MessageHandlers
{
    public class ShippingExpressAuthHandler:BasicAuthenticationHandler
    {
        protected override Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string username, string password,
            CancellationToken cancellationToken)
        {
            IMembershipService membershipService = request.GetMembershipService();
            var validUserContext = membershipService.ValidateUser(username, password);
            return Task.FromResult(validUserContext.Principal);
        }
    }

    internal class UnauthenticatedRequestContext
    {
        public UnauthenticatedRequestContext(HttpRequestMessage request)
        {
            Request = request;
        }

        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; set; }
    }

    static class HttpRequestMessageExtensions
    {
        internal static IMembershipService GetMembershipService(this HttpRequestMessage request)
        {
            return request.GetService<IMembershipService>();
        }

        internal static TService GetService<TService>(this HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService) dependencyScope.GetService(typeof (TService));
            return service;
        }
        

        //static TService GetService<TService>(this)
    }
}