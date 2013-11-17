using System.Net.Http;
using System.Security.Principal;
using NSubstitute;
using ShippingExpress.Domain.Services;
using Xunit;

namespace ShippingExpress.Tests.MessageHandlerTests
{
    public class ShippingExpressAuthHandlerTests
    {
        [Fact]
        public void AuthenticateUser_Returns_IPrincipal_For_Valid_Credentials()
        {
            var userName = "superman1";
            var password = "lois_lane_007";

            HttpRequestMessage request = ConfigureAuthInfrastructure(userName, password);
        }

        private HttpRequestMessage ConfigureAuthInfrastructure(string userName, string password)
        {
            var principal = new GenericPrincipal(new GenericIdentity(userName), new[] {"Admin"});
            var requestMessage = new HttpRequestMessage();
            var membershipService = Substitute.For<IMembershipService>();
            membershipService.When(c=>c.ValidateUser(userName,password)).Do(c=>
            {
                var args = c.Args();
                var uName = args[0].ToString();
                var pwd = args[1].ToString();

            });
            return requestMessage;
        }
    }
}
