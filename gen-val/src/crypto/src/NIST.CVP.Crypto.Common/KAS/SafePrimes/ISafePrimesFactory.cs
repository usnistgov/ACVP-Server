using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Crypto.Common.KAS.SafePrimes
{
    /// <summary>
    /// Constructs domain parameters for use with FFC.
    ///
    /// Safe Prime Groups are as per:
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar3.pdf
    /// https://tools.ietf.org/html/rfc3526#page-3
    /// https://tools.ietf.org/html/rfc7919#appendix-A.1
    /// </summary>
    public interface ISafePrimesFactory
    {
        /// <summary>
        /// Gets a <see cref="FfcDomainParameters"/> from a <see cref="SafePrime"/>.
        /// </summary>
        /// <param name="safePrime">The <see cref="SafePrime"/> to retrieve <see cref="FfcDomainParameters"/> for.</param>
        /// <returns><see cref="FfcDomainParameters"/> based on a <see cref="SafePrime"/>.</returns>
        FfcDomainParameters GetSafePrime(SafePrime safePrime);
    }
}