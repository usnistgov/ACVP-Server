using NIST.CVP.Crypto.Common.Asymmetric.DSA;

namespace NIST.CVP.Crypto.Common.KES
{
    /// Describes the methods for MQV key establishment scheme
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    /// Section 5.7.1.2 (ECC)
    public interface IMqv<in TDomainParameters, in TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Generates a shared secret Z based on DSA Domain parameters
        /// </summary>
        /// <param name="domainParameters">The domain parameters associated with the DSA instance</param>
        /// <param name="xPrivateKeyPartyA">The private key X of Party A</param>
        /// <param name="yPublicKeyPartyB">The public key Y of Party B</param>
        /// <param name="rPrivateKeyPartyA">A second private key R of Party A</param>
        /// <param name="tPublicKeyPartyA">The matching public key T (To R) of Party A</param>
        /// <param name="tPublicKeyPartyB">A second public key T of Party B</param>
        /// <returns></returns>
        SharedSecretResponse GenerateSharedSecretZ(
            TDomainParameters domainParameters,
            TKeyPair xPrivateKeyPartyA,
            TKeyPair yPublicKeyPartyB,
            TKeyPair rPrivateKeyPartyA,
            TKeyPair tPublicKeyPartyA,
            TKeyPair tPublicKeyPartyB
        );
    }
}