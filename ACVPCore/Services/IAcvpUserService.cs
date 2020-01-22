using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Services
{
    public interface IAcvpUserService
    {
        List<AcvpUserLite> GetUserList();
        AcvpUser GetUserDetails(long userId);
        Result SetUserTotpSeed(long userId, string seed);
    }
}