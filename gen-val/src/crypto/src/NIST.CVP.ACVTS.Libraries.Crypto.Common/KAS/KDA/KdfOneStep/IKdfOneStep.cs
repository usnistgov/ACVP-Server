using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep
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
        /// <param name="fixedInfo">The fixed information to be used in the KDF</param>
        /// <param name="salt">The salt used for mac based KDFs.</param>
        /// <returns></returns>
        KdfResult DeriveKey(BitString z, int keyDataLength, BitString fixedInfo, BitString salt);
    }
}
