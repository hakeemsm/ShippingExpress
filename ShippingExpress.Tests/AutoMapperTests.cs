using AutoMapper;
using ShippingExpress.API.Config;
using Xunit;

namespace ShippingExpress.Tests
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapper_Is_Configured_Correctly()
        {
            EntityMapping.ConfigureAutoMapper();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
