using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ShippingExpress.API.MessageHandlers;
using ShippingExpress.Tests.TestHelpers;
using Xunit;
using ShippingExpress.Domain.Utils;

namespace ShippingExpress.Tests.MessageHandlerTests
{
    public class BasicAuthenticationHandlerTests
    {
        internal static string UserName = "superman007";
        internal static string Password = "lois_lane_007";

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Returns_Unauthorized_If_Auth_Header_Is_Missing()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            var customBasicAuthHandler = new CustomBasicAuthHandler();
            return TestHelper.InvokeMessageHandler(requestMessage, customBasicAuthHandler).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Unauthorized);
            });

        }
    }

    internal class CustomBasicAuthHandler:BasicAuthenticationHandler
    {
        protected override Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string userName, string password,
            CancellationToken cancellation = default(CancellationToken))
        {
            if (userName==BasicAuthenticationHandlerTests.UserName && password==BasicAuthenticationHandlerTests.Password)
            {
                var identity = new GenericIdentity(userName);
                return TaskHelpers.FromResult<IPrincipal>(new GenericPrincipal(identity, null));
            }
            return TaskHelpers.FromResult<IPrincipal>(null);
        }
    }

    internal class GCForceAttribute : BeforeAfterTestAttribute
    {
        public override void After(MethodInfo methodUnderTest)
        {
            GC.Collect(3);
        }
    }

    internal class NullUpCurrentPrincipalAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            Thread.CurrentPrincipal = null;
        }
    }
}
