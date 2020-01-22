using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
    public interface IAcvpUserProvider
    {
        List<AcvpUserLite> GetUserList();
        AcvpUser GetUserDetails(long userId);
        Result SetUserTotpSeed(long userId, string seed);
    }
}