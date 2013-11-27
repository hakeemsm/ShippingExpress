using System.Data.Entity;
using System.Web.Http;
using ShippingExpress.Domain.Entities.Core;
using ShippingExpress.Domain.Services;
using StructureMap;
using StructureMap.Configuration.DSL;
using WebApiContrib.IoC.StructureMap;

namespace ShippingExpress.API.Config
{
    public class IoCConfig
    {
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices());
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new StructureMapResolver(container);
        }

        public static IContainer RegisterServices()
        {
            return new Container(x => x.AddRegistry(new ShippingIoCRegistry()));
        }
    }

    public class ShippingIoCRegistry : Registry
    {
        public ShippingIoCRegistry()
        {
            For(typeof (IEntityRepository<>)).Use(typeof (EntityRepository<>));
            For<DbContext>().HybridHttpOrThreadLocalScoped().Use<EntitiesContext>();
            For<IMembershipService>().HybridHttpOrThreadLocalScoped().Use<MembershipService>();
            For<ICryptoService>().Use<CryptoService>();
        }
    }
}
