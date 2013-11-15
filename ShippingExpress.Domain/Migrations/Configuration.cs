using System;
using ShippingExpress.Domain.Entities;

namespace ShippingExpress.Domain.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<Entities.Core.EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Entities.Core.EntitiesContext context)
        {
            context.Roles.AddOrUpdate(r => r.Name,
                new Role { Key = Guid.NewGuid(), Name = "Admin" },
                new Role { Key = Guid.NewGuid(), Name = "Employee" },
                new Role { Key = Guid.NewGuid(), Name = "Affiliate" }
                );
        }
    }
}
