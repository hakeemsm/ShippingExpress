using System.Collections.Generic;
using ShippingExpress.Domain.Entities;

namespace ShippingExpress.Domain.Services
{
    public class UserWithRoles
    {
        public User User { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}