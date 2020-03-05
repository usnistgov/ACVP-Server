using System.Collections.Generic;
using System.Threading.Tasks;
using ACVPCore.Models;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserProvider _provider;

        public AdminUserService(IAdminUserProvider provider)
        {
            _provider = provider;
        }
        
        public List<AdminUser> GetUsers()
        {
            return _provider.GetUsers();
        }

        public async Task<bool> IsUserAuthorized(string email)
        {
            return await _provider.IsUserAuthorized(email);
        }

        public void AddUser(string email)
        {
            _provider.AddUser(email);
        }
    }
}