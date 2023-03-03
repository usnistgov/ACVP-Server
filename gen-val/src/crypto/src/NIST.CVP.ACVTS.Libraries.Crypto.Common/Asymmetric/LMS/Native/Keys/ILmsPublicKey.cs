using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmsPublicKey
    {
        /// <summary>
        /// The attributes of the LMS key, describes specific hash functions as well as attributes used for
        /// key construction, signing, and verifying.
        /// </summary>
        LmsAttribute LmsAttribute { get; }

        /// <summary>
        /// Gets the public key of the LMS tree.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#appendix-C
        /// </summary>
        /// <returns>The public key of the LMS tree.</returns>
        byte[] Key { get; }
    }
}
