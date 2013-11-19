using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShippingExpress.Domain.Utils;

namespace ShippingExpress.API.MessageHandlers
{
    public abstract class BasicAuthenticationHandler:DelegatingHandler
    {
        private const char HttpCredentialSep = ':';
        private const string HttpBasicSchemeName = "Basic";

        public BasicAuthenticationHandler():this(false)
        {
            
        }

        public BasicAuthenticationHandler(bool suppressIfAuthenticated)
        {
            SuppressIfAlreadyAuthenticated = suppressIfAuthenticated;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated||!SuppressIfAlreadyAuthenticated)
            {
                if (request.Headers.Authorization!=null&&request.Headers.Authorization.Scheme==HttpBasicSchemeName)
                {
                    string userName;
                    string password;
                    if (TryExtractBasicAuthCredentialsFromHeader(request.Headers.Authorization.Parameter,out userName, out password))
                    {
                        return AuthenticateUserAsync(request, userName, password, cancellationToken).Then(p =>
                        {
                            if (p != null)
                            {
                                Thread.CurrentPrincipal = p;
                                return base.SendAsync(request, cancellationToken);
                            }
                            return HandleUnauthenticatedRequestImpl(request, cancellationToken);
                        },runSynchronously:true);
                    }
                }
                return HandleUnauthenticatedRequestImpl(request, cancellationToken);
            }

            return base.SendAsync(request, cancellationToken);
        }

        private bool TryExtractBasicAuthCredentialsFromHeader(string authHeader, out string userName, out string password)
        {
            userName = null;
            password = null;
            if (string.IsNullOrEmpty(authHeader))
                return false;
            byte[] base64String = Convert.FromBase64String(authHeader);
            string decodedAuthHeader = Encoding.UTF8.GetString(base64String, 0, base64String.Length);
            int sepPos = decodedAuthHeader.IndexOf(HttpCredentialSep);
            if (sepPos <= 0)
                return false;
            userName = decodedAuthHeader.Substring(0, sepPos).Trim();
            password = decodedAuthHeader.Substring(sepPos + 1).Trim();

            return string.IsNullOrEmpty(userName);
        }

        private Task<HttpResponseMessage> HandleUnauthenticatedRequestImpl(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var unauthenticatedRequestContext = new UnauthenticatedRequestContext(request);
                HandleUnauthenticatedRequest(unauthenticatedRequestContext);

                if (unauthenticatedRequestContext.Response!=null)
                {
                    EnsureRequestMessageExistence(unauthenticatedRequestContext.Response, request);
                    return TaskHelpers.FromResult(unauthenticatedRequestContext.Response);
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception e)
            {
                return TaskHelpers.FromError<HttpResponseMessage>(e);
            }
        }

        private void EnsureRequestMessageExistence(HttpResponseMessage response, HttpRequestMessage request)
        {
            if (response.RequestMessage == null)
                response.RequestMessage = request;
        }

        private void HandleUnauthenticatedRequest(UnauthenticatedRequestContext unauthenticatedRequestContext)
        {
            var responseMessage = unauthenticatedRequestContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            responseMessage.Headers.Add("WWW-Authenticate",HttpBasicSchemeName);
            unauthenticatedRequestContext.Response = responseMessage;
        }

        public bool SuppressIfAlreadyAuthenticated { get; set; }

        protected abstract Task<IPrincipal> AuthenticateUserAsync(HttpRequestMessage request, string userName,
            string password, CancellationToken cancellation = new CancellationToken());

    }
}