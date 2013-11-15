using System;
using System.Collections.Generic;
using System.Linq;
using ShippingExpress.Domain.Entities;
using ShippingExpress.Domain.Entities.Core;
using ShippingExpress.Domain.Entities.Extensions;

namespace ShippingExpress.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<UserInRole> _userInRoleRepository;
        private readonly ICryptoService _cryptoService;

        public MembershipService(IEntityRepository<User> userRepository, IEntityRepository<Role> roleRepository, 
            IEntityRepository<UserInRole> userInRoleRepository,
            ICryptoService cryptoService)
        {

            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _cryptoService = cryptoService;

        }

        public ValidUserContext ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public OperationResult<UserWithRoles> CreateUser(string userName, string email, string password)
        {
            return CreateUser(userName, email, password, roles: null);
        }

        public OperationResult<UserWithRoles> CreateUser(string username, string email, string password, string role)
        {
            return CreateUser(username, email, password, new[] {role});
        }

        public OperationResult<UserWithRoles> CreateUser(string username, string email, string password, string[] roles)
        {
            bool existingUser = _userRepository.GetAll().Any(u => u.Name == username);
            if (existingUser)
                return new OperationResult<UserWithRoles>(false);
            string pwdSalt = _cryptoService.GenerateSalt();
            var user = new User
            {
                Name = username,
                Email = email,
                CreatedOn = DateTime.Now,
                HashedPassword = _cryptoService.EncryptPassword(password, pwdSalt),
                IsLocked = false
            };
            _userRepository.Add(user);
            _userRepository.Save();

            if (roles != null && roles.Length > 0)
                roles.ForEach(r => AddUserToRole(user, r));

            return new OperationResult<UserWithRoles>(true){Entity = GetUserWithRoles(user)};
        }

        private UserWithRoles GetUserWithRoles(User user)
        {
            if (user != null)
            {

                var userRoles = GetUserRoles(user.Key);
                return new UserWithRoles()
                {
                    User = user,
                    Roles = userRoles
                };
            }

            return null;
        }

        private IEnumerable<Role> GetUserRoles(Guid userKey)
        {
            var userInRoles = _userInRoleRepository
               .FindBy(x => x.UserKey == userKey).ToList();

            if (userInRoles.Any())
            {

                var userRoleKeys = userInRoles.Select(
                    x => x.RoleKey).ToArray();

                var userRoles = _roleRepository
                    .FindBy(x => userRoleKeys.Contains(x.Key));

                return userRoles;
            }

            return Enumerable.Empty<Role>();
        }

        private void AddUserToRole(User user, string roleName)
        {
            var role = _roleRepository.GetSingleByRoleName(roleName);
            if (role==null)
            {
                var newRole = new Role {Name = roleName};
                _roleRepository.Add(newRole);
                _roleRepository.Save();
                role = newRole;
            }
            var userInRole = new UserInRole {RoleKey = role.Key, UserKey = user.Key};
            _userInRoleRepository.Add(userInRole);
            _roleRepository.Save();
        }

        public UserWithRoles UpdateUser(User user, string username, string email)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool AddToRole(Guid userKey, string role)
        {
            throw new NotImplementedException();
        }

        public bool AddToRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Role GetRole(Guid key)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(string name)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<UserWithRoles> GetUsers(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public UserWithRoles GetUser(Guid key)
        {
            throw new NotImplementedException();
        }

        public UserWithRoles GetUser(string name)
        {
            throw new NotImplementedException();
        }
    }
}