using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ShippingExpress.API.Model.ExtensionHelpers;
using ShippingExpress.Tests.Common;

namespace ShippingExpress.Tests.TestHelpers
{
    public static class HttpRequestMessageHelper
    {
        public static HttpRequestMessage ConstructRequest(HttpMethod httpMethod, string uri, string mediaType, string username, string password)
        {
            return ConstructRequest(httpMethod, uri, new[] {mediaType}, username, password);
        }

        private static HttpRequestMessage ConstructRequest(HttpMethod httpMethod, string uri, IEnumerable<string> mediaTypes, string username, string password)
        {
            HttpRequestMessage request = ConstructRequest(httpMethod, uri, mediaTypes);
            request.Headers.Authorization=new AuthenticationHeaderValue("Basic",EncodeToBase64(string.Format("{0}:{1}",username,password)));
            return request;
        }

        private static string EncodeToBase64(string format)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(format);
            return Convert.ToBase64String(bytes);
        }

        private static HttpRequestMessage ConstructRequest(HttpMethod httpMethod, string uri, IEnumerable<string> mediaTypes)
        {
            return ConstructRequest(httpMethod, uri, mediaTypes.ToMediaTypeWithQualityHeaderValues());
        }

        private static HttpRequestMessage ConstructRequest(HttpMethod httpMethod, string uri, IEnumerable<MediaTypeWithQualityHeaderValue> mediaTypes)
        {
            HttpRequestMessage request = ConstructRequest(httpMethod, uri);
            request.Headers.Accept.AddTo(mediaTypes);
            return request;
        }

        private static HttpRequestMessage ConstructRequest(HttpMethod httpMethod, string uri)
        {
            return new HttpRequestMessage(httpMethod,uri);
        }
    }
}