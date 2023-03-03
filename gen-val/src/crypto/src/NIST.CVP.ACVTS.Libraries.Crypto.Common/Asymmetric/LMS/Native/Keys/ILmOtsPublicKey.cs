using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmOtsPublicKey
    {
        /// <summary>
        /// The attributes of the LM-OTS key, describes specific hash functions as well as attributes used for
        /// key construction, signing, and verifying.
        /// </summary>
        LmOtsAttribute LmOtsAttribute { get; }

        /// <summary>
        /// The formatted public key where:
        /// _[0] ... _[3] -> type code - The attributes of the key
        /// _[4] ... _[19] -> I - The 16 byte merkle tree identifier.
        /// _[20] ... _[23] -> Q - The 32 bit merkle tree leaf indicator.
        /// _[24] ... _[24+n] K - The hash of the concatenation of values, namely the tree attributes and private key elements.
        ///
        /// As specified from algorithm 1 in https://datatracker.ietf.org/doc/html/rfc8554#section-4.3
        /// </summary>
        byte[] Key { get; }

        /// <summary>
        /// The "K" portion of Algorithm 1 specified in https://datatracker.ietf.org/doc/html/rfc8554#section-4.3
        /// </summary>
        byte[] K { get; }
    }
}
