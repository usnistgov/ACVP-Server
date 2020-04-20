using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPCore.Providers
{
    public interface IAcvpUserProvider
    {
        PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param);
        AcvpUser GetUserDetails(long userId);
        Result SetUserTotpSeed(long userId, string seed);
        Result CreateUser(string personName, long organizationID, byte[] certificate);
        Result SetUserCertificate(long userId, byte[] certificate);
        Result DeleteUser(long userId);
    }
}