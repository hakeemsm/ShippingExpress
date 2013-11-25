using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Services
{
    public interface IShipmentService
    {
        PaginatedList<Shipment> GetShipments(int page, int take);
    }
}