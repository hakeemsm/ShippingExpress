using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingExpress.API.Model.RequestCommands
{
    public class ShipmentRequestModel:ShipmentBaseRequestModel
    {
        [Required]
        public Guid? AffiliateKey { get; set; }
        [Required]
        public Guid? ShipmentTypeKey { get; set; }
    }

    public class ShipmentBaseRequestModel
    {
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public string ReceiverFirstName { get; set; }
        [Required]
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
        [EmailAddress]
        [StringLength(320)]
        public string ReceiverEmail { get; set; }
    }
}
