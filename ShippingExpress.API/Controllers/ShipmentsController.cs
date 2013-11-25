using System.Web.Http;
using AutoMapper;
using ShippingExpress.API.Model.Dtos;
using ShippingExpress.API.Model.RequestCommands;
using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Core;
using ShippingExpress.Domain.Services;

namespace ShippingExpress.API.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ShipmentsController:ApiController
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        public PaginatedDto<ShipmentDto> GetShipments(PaginatedRequestCommand cmd)
        {
            PaginatedList<Shipment> shipments = _shipmentService.GetShipments(cmd.Page, cmd.Take);
            return Mapper.Map<PaginatedList<Shipment>, PaginatedDto<ShipmentDto>>(shipments);
        }
    }
}
