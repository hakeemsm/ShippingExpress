using System;

namespace ShippingExpress.Domain.Entities.Core
{
    public interface IEntity
    {
        Guid Key { get; set; }         
    }

}
