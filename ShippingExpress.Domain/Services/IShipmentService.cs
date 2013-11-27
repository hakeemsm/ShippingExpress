using System;
using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Services
{
    public interface IShipmentService
    {
        PaginatedList<Shipment> GetShipments(int page, int take);
        Shipment GetShipment(Guid shipmentKey);
    }

    public class ShipmentService : IShipmentService
    {
        public PaginatedList<Shipment> GetShipments(int page, int take)
        {
            throw new NotImplementedException();
        }

        public Shipment GetShipment(Guid shipmentKey)
        {
            throw new NotImplementedException();
        }
    }
}