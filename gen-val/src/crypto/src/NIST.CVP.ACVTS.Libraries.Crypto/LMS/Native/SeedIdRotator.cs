using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public class SeedIdRotator : ISeedIdRotator
    {
        public virtual IdSeedResult GetNewSeedId(ISha sha, LmsAttribute lmsAttribute, byte[] seed, byte[] i, int level)
        {
            return LmsHelpers.CalculateNewSeedIdFromExisting(sha, lmsAttribute, i, seed, level);
        }
    }
}
