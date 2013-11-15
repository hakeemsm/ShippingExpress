using System.Linq;
using ShippingExpress.Domain.Entities.Core;

namespace ShippingExpress.Domain.Entities.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static User GetSingleUserByName(this IEntityRepository<User> userRepository, string userName)
        {
            return userRepository.GetAll().FirstOrDefault(u => u.Name == userName);
        }
    }
}
