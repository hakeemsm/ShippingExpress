using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities
{
    public class Affiliate:IEntity
    {
        public Affiliate()
        {
            Shipments = new HashSet<Shipment>();
        }

        public virtual ICollection<Shipment> Shipments { get; set; }

        [Key]
        public Guid Key { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TelephoneNumber { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public User User { get; set; }


    }
}
