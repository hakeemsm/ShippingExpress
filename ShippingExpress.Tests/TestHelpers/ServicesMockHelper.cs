using System.Linq;
using System.Security.Principal;
using NSubstitute;
using ShippingExpress.Domain.Services;
using ShippingExpress.Tests.ControllerTests;

namespace ShippingExpress.Tests.TestHelpers
{
    internal class ServicesMockHelper
    {
        internal static IMembershipService GetMembershipServiceMock()
        {
            var membershipService = Substitute.For<IMembershipService>();
            var users = new[] { 
                new { 
                    Name = Constants.ValidAdminUserName, 
                    Password = Constants.ValidAdminPassword, 
                    Roles = new[] { "Admin" } 
                },
                new { 
                    Name = Constants.ValidEmployeeUserName, 
                    Password = Constants.ValidEmployeePassword, 
                    Roles = new[] { "Employee" } 
                },
                new { 
                    Name = Constants.ValidAffiliateUserName, 
                    Password = Constants.ValidAffiliatePassword, 
                    Roles = new[] { "Affiliate" } 
                }
            }.ToDictionary(user => user.Name, user => user);
            membershipService.ValidateUser(Arg.Any<string>(), Arg.Any<string>()).Returns(info =>
            {
                var args = info.Args();
                var uName = args[0].ToString();
                var pwd = args[1].ToString();
                var user = users.FirstOrDefault(x => x.Key.Equals(uName)).Value;
                var validUserContext = user != null
                    ? new ValidUserContext
                    {
                        Principal = new GenericPrincipal(new GenericIdentity(user.Name), user.Roles)
                    }
                    : new ValidUserContext();
                return validUserContext;
            });
            return membershipService;
        }
    }
}