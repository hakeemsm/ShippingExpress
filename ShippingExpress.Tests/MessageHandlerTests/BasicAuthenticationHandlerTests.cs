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

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Current_Thread_Principal_Set_When_Authorization_Header_Is_Verified()
        {
            string userNamePwd = string.Format("{0}:{1}", UserName, Password);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            requestMessage.Headers.Authorization=new AuthenticationHeaderValue("Basic",EncodeToBase64(userNamePwd));
            var customBasicAuthHandler = new CustomBasicAuthHandler();
            return TestHelper.InvokeMessageHandler(requestMessage, customBasicAuthHandler).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                Thread.CurrentPrincipal.Should().NotBeNull();
                Thread.CurrentPrincipal.Should().BeAssignableTo<GenericPrincipal>();
                
            });
        }

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Suppresses_Auth_If_Already_Authenticated_And_Suppress_Requested()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            var authHandler = new SuppressedCustomBasicAuthHandler();
            return TestHelper.InvokeMessageHandler(requestMessage, authHandler).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                authHandler.IsAuthenticateUserCalled.Should().BeFalse();
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            });
        }

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Overridden_Handle_Unauthenticated_Request_Is_Honored_And_Response_Set()
        {
            string userNamePwd = string.Format("{0}:{1}", UserName, Password);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(userNamePwd));
            var unauthImpl = new CustomBasicAuthHandlerWithUnauthImpl();

            return TestHelper.InvokeMessageHandler(requestMessage, unauthImpl).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Ambiguous);

            });
        }

        [Fact, NullUpCurrentPrincipal, GCForce]
        public Task Overridden_Handle_Unauthenticated_Request_Is_Honored_And_InnerHandler_Called()
        {
            string userNamePwd = string.Format("{0}:{1}", UserName, Password);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(userNamePwd));
            var emptyUnauthImpl = new CustomBasicAuthHandlerWithEmptyUnauthImpl();

            return TestHelper.InvokeMessageHandler(requestMessage, emptyUnauthImpl).ContinueWith(t =>
            {
                t.Status.ShouldBeEquivalentTo(TaskStatus.RanToCompletion);
                t.Result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
                Thread.CurrentPrincipal.Identity.IsAuthenticated.Should().BeFalse();
            });
        }

        private string EncodeToBase64(string value)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytesToEncode);
        }
    }

    public class CustomBasicAuthHandlerWithEmptyUnauthImpl:BasicAuthenticationHandler
    {
        protected override Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string userName, string password,
            CancellationToken cancellation = new CancellationToken())
        {
            return TaskHelpers.FromResult<IPrincipal>(null);
        }

        protected override void HandleUnauthenticatedRequest(UnauthenticatedRequestContext unauthenticatedRequestContext)
        {
            
        }
    }

    public class CustomBasicAuthHandlerWithUnauthImpl:BasicAuthenticationHandler
    {
        protected override Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string userName, string password,
            CancellationToken cancellation = new CancellationToken())
        {
            return TaskHelpers.FromResult<IPrincipal>(null);
        }

        protected override void HandleUnauthenticatedRequest(UnauthenticatedRequestContext context)
        {
            context.Response=new HttpResponseMessage(HttpStatusCode.Ambiguous);
        }
    }

    public class SuppressedCustomBasicAuthHandler:BasicAuthenticationHandler
    {

        public SuppressedCustomBasicAuthHandler():base(true)
        {
            
        }
        protected override Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string userName, string password,
            CancellationToken cancellation = new CancellationToken())
        {
            IsAuthenticateUserCalled = true;
            return TaskHelpers.FromResult<IPrincipal>(null);
        }

        public bool IsAuthenticateUserCalled { get; private set; }
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
