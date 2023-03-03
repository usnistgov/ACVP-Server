using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Provides a need seed and id to use
    /// </summary>
    public interface ISeedIdRotator
    {
        /// <summary>
        /// Get a new seed and id based on a LMS attributes and the current seed/id
        /// </summary>
        /// <param name="sha">The sha instance used to construct the new seed and id.</param>
        /// <param name="lmsAttribute">The attributes used in the LMS tree.</param>
        /// <param name="seed">The current seed.</param>
        /// <param name="i">The current seed.</param>
        /// <param name="level">The HSS level in which the seed/id are being generated.</param>
        /// <returns>A new seed and id.</returns>
        IdSeedResult GetNewSeedId(ISha sha, LmsAttribute lmsAttribute, byte[] seed, byte[] i, int level);
    }
}
