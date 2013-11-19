using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using FluentAssertions;
using NSubstitute;
using ShippingExpress.API.MessageHandlers;
using ShippingExpress.Domain.Services;
using Xunit;

namespace ShippingExpress.Tests.MessageHandlerTests
{
    public class ShippingExpressAuthHandlerTests
    {
        [Fact]
        public async Task AuthenticateUser_Returns_IPrincipal_For_Valid_Credentials()
        {
            var userName = "superman1";
            var password = "lois_lane_007";
            var authHandler = new BasicAuthHandlerTestHelper();
            HttpRequestMessage request = ConfigureAuthInfrastructure(userName, password);
            IPrincipal principal = await authHandler.RunAuthenticateUserMethodAsync(request, userName, password, default(CancellationToken));
            principal.Identity.Name.Should().BeEquivalentTo(userName);
        }

        private HttpRequestMessage ConfigureAuthInfrastructure(string userName, string password)
        {
            var principal = new GenericPrincipal(new GenericIdentity(userName), new[] {"Admin"});
            var requestMessage = new HttpRequestMessage();
            var membershipService = Substitute.For<IMembershipService>();

            membershipService.ValidateUser(userName, password).ReturnsForAnyArgs(info =>
            {
                var args = info.Args();
                var uName = args[0].ToString();
                var pwd = args[1].ToString();
                return uName == userName && pwd == password
                    ? new ValidUserContext {Principal = principal}
                    : new ValidUserContext();
            });
            var dependencyScope = Substitute.For<IDependencyScope>();
            dependencyScope.GetService(typeof (IMembershipService)).Returns(membershipService);
            requestMessage.Properties[HttpPropertyKeys.DependencyScope] = dependencyScope;
            return requestMessage;
        }
    }

    public class BasicAuthHandlerTestHelper:ShippingExpressAuthHandler
    {
        public Task<IPrincipal> RunAuthenticateUserMethodAsync(HttpRequestMessage request, string userName, string password, CancellationToken cancellationToken)
        {
            return AuthenticateUserAsync(request, userName, password, cancellationToken);
        }
    }
}
