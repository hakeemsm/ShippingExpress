using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ShippingExpress.API.MessageHandlers;
using ShippingExpress.Tests.TestHelpers;
using Xunit;

namespace ShippingExpress.Tests.MessageHandlerTests
{
    public class RequireHttpsMessageHandlerTest
    {
        [Fact]
        public async Task Returns_Forbidden_If_Request_Is_Not_Http()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080");
            var messageHandler = new RequireHttpsMessageHandler();

            var responseMessage = await messageHandler.InvokeAsync(requestMessage);
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Returns_StatusCode_OK_For_Https_Request()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:8080");
            var messageHandler = new RequireHttpsMessageHandler();

            var responseMessage = await messageHandler.InvokeAsync(requestMessage);
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
