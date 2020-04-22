using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
    public interface IAcvpUserService
    {
        PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param);
        AcvpUser GetUserDetails(long userId);
        Result SetUserTotpSeed(long userId, AcvpUserSeedUpdateParameters param);
        Result RefreshTotpSeed(long userId);
        Result CreateUser(AcvpUserCreateParameters param);
        Result SetUserCertificate(long userId, AcvpUserCertificateUpdateParameters param);
        Result DeleteUser(long userId);
    }
}