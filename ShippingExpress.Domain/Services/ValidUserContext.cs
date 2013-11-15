﻿using System.Security.Principal;

namespace ShippingExpress.Domain.Services
{
    public class ValidUserContext
    {
        public IPrincipal Principal { get; set; }
        public UserWithRoles User { get; set; }

        public bool IsValid()
        {

            return Principal != null;
        }
    }
}