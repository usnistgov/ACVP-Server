using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    /// <summary>
    /// Describes methods for invoking KDFs
    /// </summary>
    public interface IKdf
    {
        /// <summary>
        /// Used to Derive a key for use in KAS
        /// </summary>
        /// <param name="z">The shared secret Z</param>
        /// <param name="keyDataLength">The output length of the keying material</param>
        /// <param name="otherInfoStrategy">The other information strategy to be used in the KDF</param>
        /// <returns></returns>
        KdfResult DeriveKey(BitString z, int keyDataLength, IOtherInfoStrategy otherInfoStrategy);
    }
}