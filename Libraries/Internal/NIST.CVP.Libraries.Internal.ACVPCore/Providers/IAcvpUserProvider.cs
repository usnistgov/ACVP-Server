using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
    public interface IAcvpUserProvider
    {
        PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param);
        AcvpUser GetUserDetails(long userId);
        Result SetUserTotpSeed(long userId, string seed);
        Result CreateUser(string personName, long organizationID, byte[] certificate, string[] personEmails);
        Result SetUserCertificate(long userId, byte[] certificate);
        Result DeleteUser(long userId);
    }
}