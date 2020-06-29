using System;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IAcvpUserProvider
    {
        PagedEnumerable<AcvpUserLite> GetList(AcvpUserListParameters param);
        AcvpUser Get(long userId);
        Result UpdateSeed(long userId, string seed);
        Result Insert(long personID, string subject, byte[] rawData, string seed, DateTime expiresOn);
        Result UpdateCertificate(long userId, string subject, byte[] rawData, DateTime expiresOn);
        Result Delete(long userId);
    }
}