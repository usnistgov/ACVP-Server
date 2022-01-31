using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KES
{
    /// <summary>
    /// Describes the methods for Diffie Hellman key establishment scheme
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    /// Section 5.7.1.2 (ECC)
    /// </summary>
    public interface IDiffieHellman<in TDomainParameters, in TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Generates a shared secret Z based on DSA Domain parameters, 
        /// a private key from party A, 
        /// and public key from party B
        /// </summary>
        /// <param name="domainParameters">The domain parameters of the DSA instance</param>
        /// <param name="xPrivateKeyPartyA">The private key X of Party A</param>
        /// <param name="yPublicKeyPartyB">The public key Y of Party B</param>
        /// <returns></returns>
        SharedSecretResponse GenerateSharedSecretZ(
            TDomainParameters domainParameters,
            TKeyPair xPrivateKeyPartyA,
            TKeyPair yPublicKeyPartyB
        );
    }
}
