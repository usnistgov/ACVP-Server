using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
    public class AcvpUserService : IAcvpUserService
    {
        private readonly IAcvpUserProvider _adminUserProvider;

        public AcvpUserService(IAcvpUserProvider adminUserProvider)
        {
            _adminUserProvider = adminUserProvider;
        }
        
        public PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param)
        {
            return _adminUserProvider.GetUserList(param);
        }

        public AcvpUser GetUserDetails(long userId)
        {
            return _adminUserProvider.GetUserDetails(userId);
        }

        public Result SetUserTotpSeed(long userId, string seed)
        {
            return _adminUserProvider.SetUserTotpSeed(userId, seed);
        }
    }
}