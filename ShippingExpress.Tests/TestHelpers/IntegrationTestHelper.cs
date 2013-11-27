using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using ShippingExpress.API.Config;
using ShippingExpress.API.Model.Dtos;
using StructureMap;

namespace ShippingExpress.Tests.TestHelpers
{
    public class IntegrationTestHelper
    {
        public static HttpConfiguration GetInitialIntegrationTestConfig(IContainer container)
        {
            HttpConfiguration config = GetInitialIntegrationTestConfig();
            IoCConfig.Initialize(config,container);
            return config;
        }

        private static HttpConfiguration GetInitialIntegrationTestConfig()
        {
            var configuration = new HttpConfiguration();
            RouteConfig.RegisterRoutes(configuration);
            WebAPIConfig.Configure(configuration);
            return configuration;
        }

        public static async Task<PaginatedDto<TDto>> TestPaginatedDtoAsync<TDto>(HttpConfiguration httpConfig, HttpRequestMessage request, 
            int expectedPageIdx, int expectedTotalPageCount, int expectedCurrentItemsCount, int expectedTotalItemsCount, 
            bool expectedHasNextPageResult, bool expectedHasPreviousPageResult) where TDto:IDto
        {
            PaginatedDto<TDto> paginatedDto =
                await GetResponseMessageBodyAsync<PaginatedDto<TDto>>(httpConfig, request, HttpStatusCode.OK);
            paginatedDto.PageIndex.ShouldBeEquivalentTo(expectedPageIdx);
            paginatedDto.TotalPageCount.ShouldBeEquivalentTo(expectedTotalPageCount);
            paginatedDto.Items.Count().ShouldBeEquivalentTo(expectedCurrentItemsCount);
            paginatedDto.TotalCount.ShouldBeEquivalentTo(expectedTotalItemsCount);
            paginatedDto.HasNextPage.ShouldBeEquivalentTo(expectedHasNextPageResult);
            paginatedDto.HasPreviousPage.ShouldBeEquivalentTo(expectedHasPreviousPageResult);
            return paginatedDto;
        }

        private static async Task<TResult> GetResponseMessageBodyAsync<TResult>(HttpConfiguration httpConfig, HttpRequestMessage request, HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage response = await GetResponseAsync(httpConfig, request);
            response.StatusCode.ShouldBeEquivalentTo(expectedStatusCode);
            return await response.Content.ReadAsAsync<TResult>();
        }

        private static async Task<HttpResponseMessage> GetResponseAsync(HttpConfiguration httpConfig, HttpRequestMessage request)
        {
            using (var httpServer = new HttpServer(httpConfig))
            using(var client = HttpClientFactory.Create(httpServer))
            {
                return await client.SendAsync(request);
            }
        }
    }
}
