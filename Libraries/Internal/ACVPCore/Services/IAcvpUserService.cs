using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;


namespace ACVPCore.Services
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