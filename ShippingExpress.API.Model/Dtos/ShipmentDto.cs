using System;
using System.Collections.Generic;

namespace ShippingExpress.API.Model.Dtos
{
    public class ShipmentDto:IDto
    {
        public Guid Key { get; set; }
        public Guid AffiliateKey { get; set; }

        public decimal Price { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastname { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverZipCode { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverEmail { get; set; }
        public DateTime CreatedOn { get; set; }

        public ShipmentTypeDto ShipmentType { get; set; }
        public IEnumerable<ShipmentStateDto>
            ShipmentStates { get; set; }
    }

    public class ShipmentTypeDto:IDto
    {
        public Guid Key { get; set; }
        public string CompanyName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
