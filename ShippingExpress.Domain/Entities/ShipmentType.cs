using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities
{
    public class ShipmentType:IEntity
    {
        public ShipmentType()
        {
            Shipments = new HashSet<Shipment>();
        }

        public virtual ICollection<Shipment> Shipments { get; set; }

        [Key]
        public Guid Key { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
