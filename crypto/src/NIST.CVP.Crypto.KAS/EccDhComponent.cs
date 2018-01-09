using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KES;

namespace NIST.CVP.Crypto.KAS
{
    public class EccDhComponent : IEccDhComponent
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public EccDhComponent(IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman)
        {
            _diffieHellman = diffieHellman;
        }

        public SharedSecretResponse GenerateSharedSecret(EccDomainParameters domainParameters, EccKeyPair privateKeyPartyA, EccKeyPair publicKeyPartyB)
        {
            return _diffieHellman.GenerateSharedSecretZ(domainParameters, privateKeyPartyA, publicKeyPartyB);
        }
    }
}