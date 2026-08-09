using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys
{
    public interface IXmssPublicKey
    {
        /// <summary>
        /// The attributes of the XMSS key, describes specific hash functions as well as attributes used for
        /// key construction, signing, and verifying.
        /// </summary>
        XmssAttribute XmssAttribute { get; }

        /// <summary>
        /// Gets the full wire representation of the public key, OID || root || SEED.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.7
        /// </summary>
        /// <returns>The public key of the XMSS tree.</returns>
        byte[] Key { get; }
    }
}
