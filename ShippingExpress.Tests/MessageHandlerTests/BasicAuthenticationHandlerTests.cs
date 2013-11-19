using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Principal;
using System.Text;
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

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Returns_Unauthorized_If_Authorization_Header_Is_Not_Verified()
        {
            string userNamePwd = string.Format("{0}:{1}", "user1", "rockstar");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            requestMessage.Headers.Authorization=new AuthenticationHeaderValue("Basic",EncodeToBase64(userNamePwd));
            var customBasicAuthHandler = new CustomBasicAuthHandler();

            return TestHelper.InvokeMessageHandler(requestMessage, customBasicAuthHandler).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Unauthorized);
            });
        }

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task StatusCode_OK_If_Authorization_Verified()
        {
            string userNamePwd = string.Format("{0}:{1}", UserName, Password);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(userNamePwd));
            var customBasicAuthHandler = new CustomBasicAuthHandler();
            return TestHelper.InvokeMessageHandler(requestMessage, customBasicAuthHandler).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            });

        }

        private string EncodeToBase64(string value)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytesToEncode);
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
