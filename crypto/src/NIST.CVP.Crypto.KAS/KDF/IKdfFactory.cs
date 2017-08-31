using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.KAS.KDF
{
    /// <summary>
    /// Describes retrieving a <see cref="IKdf"/> instance
    /// </summary>
    public interface IKdfFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IKdf"/> based on the specified parameters.
        /// </summary>
        /// <param name="kdfHashMode">The type of KDF</param>
        /// <param name="hashFunction">The hash function options</param>
        /// <returns></returns>
        IKdf GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction);
    }
}