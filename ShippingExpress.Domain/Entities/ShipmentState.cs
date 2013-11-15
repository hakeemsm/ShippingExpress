using System;
using System.ComponentModel.DataAnnotations;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities
{
    public class ShipmentState:IEntity
    {
        [Key]
        public Guid Key { get; set; }
        public Guid ShipmentKey { get; set; }

        [Required]
        public ShipmentStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
        public Shipment Shipment { get; set; }
    }

    public enum ShipmentStatus
    {
        Ordered = 1,
        Scheduled = 2,
        InTransit = 3,
        Delivered = 4
    }
}
