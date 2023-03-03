using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmOtsPrivateKey
    {
        /// <summary>
        /// The attributes of the LM-OTS key, describes specific hash functions as well as attributes used for
        /// key construction, signing, and verifying.
        /// </summary>
        LmOtsAttribute LmOtsAttribute { get; }
        /// <summary>
        /// The 16 byte merkle tree identifier.
        /// </summary>
        byte[] I { get; }
        /// <summary>
        /// The 32 bit merkle tree leaf indicator.
        /// </summary>
        byte[] Q { get; }
        /// <summary>
        /// The seed used to construct the key.
        /// </summary>
        byte[] Seed { get; }

        /// <summary>
        /// The p, n-byte strings making up the private key
        /// </summary>
        byte[][] X { get; }

        /// <summary>
        /// The full concatenation of the key attributes along with the constructed X values that make up the key.
        /// </summary>
        byte[] Key { get; }
    }
}
