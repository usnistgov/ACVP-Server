using System.Numerics;
using NIST.CVP.Crypto.DSA2;

namespace NIST.CVP.Crypto.KES
{
    /// <summary>
    /// Describes the methods for Diffie Hellman key establishment scheme
    /// </summary>
    public interface IDiffieHellman<TDsaDomainParameters>
        where TDsaDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// Generates a shared secret Z based on DSA Domain parameters, 
        /// a private key from party A, 
        /// and public key from party B
        /// </summary>
        /// <param name="domainParameters">DSA domain parameters</param>
        /// <param name="xPrivateKeyPartyA">The private key X of Party A</param>
        /// <param name="yPublicKeyPartyB">The public key Y of Party B</param>
        /// <returns></returns>
        DiffieHellmanResponse GenerateSharedSecretZ(
            TDsaDomainParameters domainParameters, 
            BigInteger xPrivateKeyPartyA,
            BigInteger yPublicKeyPartyB
        );
    }
}