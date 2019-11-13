using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS
{
    public interface IIfcSecretKeyingMaterial
    {
        /// <summary>
        /// The RSA Key used by this party
        /// </summary>
        KeyPair Key { get; set; }
        /// <summary>
        /// The secret to derive a key from
        /// </summary>
        BitString Z { get; set; }
        /// <summary>
        /// The nonce that goes into a KDF
        /// </summary>
        BitString DkmNonce { get; set; }
        /// <summary>
        /// This party id
        /// </summary>
        BitString PartyId { get; set; }
        /// <summary>
        /// The encrypted/encoded ciphertext representing at least a portion of a shared secret.
        /// </summary>
        BitString C { get; set; }
        /// <summary>
        /// The raw key that is to be wrapped for a KTS scheme 
        /// </summary>
        BitString K { get; set; }
    }
}