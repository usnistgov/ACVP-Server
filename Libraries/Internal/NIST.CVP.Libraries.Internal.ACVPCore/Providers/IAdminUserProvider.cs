using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
    public interface IAdminUserProvider
    {
        List<AdminUser> GetUsers();
        Task<bool> IsUserAuthorized(string email);
        void AddUser(string email);
    }
}