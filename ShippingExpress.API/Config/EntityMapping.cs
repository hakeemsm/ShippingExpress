using AutoMapper;
using ShippingExpress.API.Model.Dtos;
using ShippingExpress.Domain.Entities;

namespace ShippingExpress.API.Config
{
    public class EntityMapping
    {
        public static void ConfigureAutoMapper()
        {
            Mapper.CreateMap<Shipment, ShipmentDto>().ForSourceMember("ShipmentTypeKey",s=>s.Ignore()).ForSourceMember("Affiliate",s=>s.Ignore());
            Mapper.CreateMap<ShipmentType, ShipmentTypeDto>().ForSourceMember("Shipments", s => s.Ignore());
            Mapper.CreateMap<ShipmentState, ShipmentStateDto>()
                .ForMember("ShipmentStatus", s => s.MapFrom(st => st.Status));

        }
    }
}