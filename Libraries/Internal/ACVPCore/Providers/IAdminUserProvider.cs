using System.Collections.Generic;
using System.Threading.Tasks;
using ACVPCore.Models;

namespace ACVPCore.Providers
{
    public interface IAdminUserProvider
    {
        List<AdminUser> GetUsers();
        Task<bool> IsUserAuthorized(string email);
        void AddUser(string email);
    }
}