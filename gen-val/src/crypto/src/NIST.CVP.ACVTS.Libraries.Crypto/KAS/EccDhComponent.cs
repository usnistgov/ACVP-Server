using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS
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
