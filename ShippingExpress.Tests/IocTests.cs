using FluentAssertions;
using ShippingExpress.API.Config;
using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Core;
using Xunit;

namespace ShippingExpress.Tests
{
    public class IocTests
    {
        [Fact]
        public void IoC_Container_Has_All_Dependencies_Wired()
        {
            var container = IoCConfig.RegisterServices();
            container.AssertConfigurationIsValid();
            //container.GetInstance<IEntityRepository<User>>().Should().NotBeNull();

        }
    }
}
