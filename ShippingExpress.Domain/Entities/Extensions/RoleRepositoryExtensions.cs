using System.Linq;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities.Extensions
{
    public static class RoleRepositoryExtensions
    {
        public static Role GetSingleByRoleName(this IEntityRepository<Role> roleRepository, string roleName)
        {
            return roleRepository.GetAll().FirstOrDefault(x => x.Name == roleName);
        }
    }
}
