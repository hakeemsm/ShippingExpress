using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using ShippingExpress.API.Model.Dtos;
using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Extensions;
using ShippingExpress.Domain.Services;
using ShippingExpress.Tests.Common;
using ShippingExpress.Tests.MessageHandlerTests;
using ShippingExpress.Tests.TestHelpers;
using StructureMap;
using Xunit;

namespace ShippingExpress.Tests.ControllerTests
{
    public class ShipmentControllerIntegrationTests
    {
        private const string ApiBaseRequestPath = "api/shipments";

        [Fact, NullUpCurrentPrincipal]
        public Task Returns_200_And_Shipments_If_Request_Authorized()
        {
            HttpConfiguration httpConfig = IntegrationTestHelper.GetInitialIntegrationTestConfig(GetContainer());
            HttpRequestMessage request = HttpRequestMessageHelper.ConstructRequest(HttpMethod.Get, string.Format("https://localhost/{0}?page={1}&take={2}", ApiBaseRequestPath, 1, 1), 
                "application/json", Constants.ValidAdminUserName, Constants.ValidAdminPassword);
            return IntegrationTestHelper.TestPaginatedDtoAsync<ShipmentDto>(httpConfig, request, 1, 2, 2, 3, true, false);
        }

        private IContainer GetContainer()
        {
            IEnumerable<Shipment> shipments = GetDummyShipments(new[] {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()});
            IShipmentService shipmentService = Substitute.For<IShipmentService>();
            shipmentService.GetShipments(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(c =>
            {
                var args = c.Args();
                int pageIdx = (int)args[0];
                int pageSz = (int)args[1];
                return shipments.AsQueryable().ToPaginatedList(pageIdx, pageSz);
            });
            return GetContainerWithMocks(shipmentService);
        }

        private IContainer GetContainerWithMocks(IShipmentService shipmentService)
        {
            return new Container(x =>
            {
                x.For<IMembershipService>().Use(ServicesMockHelper.GetMembershipServiceMock());
                x.For<IShipmentService>().Use(shipmentService);
            });
        }

        private IEnumerable<Shipment> GetDummyShipments(Guid[] keys)
        {
            var shipmentKeys = new[] {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};
            for (int i = 0; i < keys.Count(); i++)
            {
                yield return new Shipment
                {
                    Key = keys[i],
                    AffiliateKey = Guid.NewGuid(),
                    ShipmentTypeKey = shipmentKeys[i],
                    Price = 12.23M*(i + 1),
                    ReceiverFirstName = string.Format("Receiver {0} FirstName", i),
                    ReceiverLastName = string.Format("Receiver {0} Lastname", i),
                    ReceiverAddress = string.Format("Receiver {0} Address", i),
                    ReceiverCity = string.Format("Receiver {0} City", i),
                    ReceiverCountry = string.Format("Receiver {0} Country", i),
                    ReceiverPhone = string.Format("Receiver {0} Phone", i),
                    ReceiverZipCode = "12345",
                    ReceiverEmail = "hotty@coolmail.com",
                    CreatedOn = DateTime.Now,
                    ShipmentType = new ShipmentType
                    {
                        Key = shipmentKeys[i],
                        CompanyName = "Nile",
                        Price = 4.19M,
                        CreatedOn = DateTime.Now,
                    },
                    ShipmentStates = new List<ShipmentState>
                    {
                        new ShipmentState
                        {
                            Key = Guid.NewGuid(),
                            ShipmentKey = keys[i],
                            Status = ShipmentStatus.Ordered
                        },
                        new ShipmentState
                        {
                            Key = Guid.NewGuid(),
                            ShipmentKey = keys[i],
                            Status = ShipmentStatus.Scheduled
                        }
                    }
                };
            }
        }
    }
}
