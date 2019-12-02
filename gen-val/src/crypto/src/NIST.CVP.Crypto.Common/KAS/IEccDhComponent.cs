using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KES;

namespace NIST.CVP.Crypto.Common.KAS
{
    /// <summary>
    /// Interface used to perform a ECC DH component test, using two <see cref="EccKeyPair"/>s
    /// </summary>
    public interface IEccDhComponent
    {
        /// <summary>
        /// Generates a shared secret based on two <see cref="EccKeyPair"/>s
        /// </summary>
        /// <param name="domainParameters">The domain parameters (curve information)</param>
        /// <param name="privateKeyPartyA">The key pair for party A</param>
        /// <param name="publicKeyPartyB">The key pair for party B</param>
        /// <returns></returns>
        SharedSecretResponse GenerateSharedSecret(EccDomainParameters domainParameters, EccKeyPair privateKeyPartyA, EccKeyPair publicKeyPartyB);
    }
}