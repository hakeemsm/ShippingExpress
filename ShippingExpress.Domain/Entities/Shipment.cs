using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities
{
    public class Shipment:IEntity
    {
        public Shipment()
        {
            ShipmentStates = new HashSet<ShipmentState>();
        }

        public ICollection<ShipmentState> ShipmentStates { get; set; }

        [Key]
        public Guid Key { get; set; }
        public Guid AffiliateKey { get; set; }
        public Guid ShipmentTypeKey { get; set; }

        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverCity { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverCountry { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverPhone { get; set; }

        [Required]
        [StringLength(300)]
        public string ReceiverEmail { get; set; }

        public DateTime CreatedOn { get; set; }
        public Affiliate Affiliate { get; set; }
        public ShipmentType ShipmentType { get; set; }
    }
}
