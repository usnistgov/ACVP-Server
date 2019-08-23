using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Describes methods for invoking KDFs
    /// </summary>
    public interface IKdfOneStep
    {
        /// <summary>
        /// Used to Derive a key for use in KAS
        /// </summary>
        /// <param name="z">The shared secret Z</param>
        /// <param name="keyDataLength">The output length of the keying material</param>
        /// <param name="otherInfo">The other information to be used in the KDF</param>
        /// <returns></returns>
        KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo);
    }
}