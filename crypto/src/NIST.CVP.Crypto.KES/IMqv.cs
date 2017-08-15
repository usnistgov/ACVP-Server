using System.Numerics;
using NIST.CVP.Crypto.DSA2;

namespace NIST.CVP.Crypto.KES
{
    /// Describes the methods for MQV key establishment scheme
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    /// Section 5.7.1.2 (ECC)
    /// <typeparam name="TDsaDomainParameters">The <see cref="IDsa"/> Domain Parameters</typeparam>
    public interface IMqv<in TDsaDomainParameters>
        where TDsaDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// Generates a shared secret Z based on DSA Domain parameters
        /// </summary>
        /// <param name="domainParameters">DSA domain parameters</param>
        /// <param name="xPrivateKeyPartyA">The private key X of Party A</param>
        /// <param name="yPublicKeyPartyB">The public key Y of Party B</param>
        /// <param name="rPrivateKeyPartyA">A second private key R of Party A</param>
        /// <param name="tPublicKeyPartyA">The matching public key T (To R) of Party A</param>
        /// <param name="tPublicKeyPartyB">A second public key T of Party B</param>
        /// <returns></returns>
        DiffieHellmanResponse GenerateSharedSecretZ(
            TDsaDomainParameters domainParameters,
            BigInteger xPrivateKeyPartyA,
            BigInteger yPublicKeyPartyB,
            BigInteger rPrivateKeyPartyA,
            BigInteger tPublicKeyPartyA,
            BigInteger tPublicKeyPartyB
        );
    }
}