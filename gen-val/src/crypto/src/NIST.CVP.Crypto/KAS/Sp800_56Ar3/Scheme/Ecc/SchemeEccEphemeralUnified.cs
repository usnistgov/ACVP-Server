using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal class SchemeEccEphemeralUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccEphemeralUnified(
            SchemeParameters schemeParameters, 
            ISecretKeyingMaterial thisPartyKeyingMaterial, 
            IFixedInfoFactory fixedInfoFactory, 
            FixedInfoParameter fixedInfoParameter, 
            IKdfFactory kdfFactory, 
            IKdfParameter kdfParameter, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            MacParameters keyConfirmationParameter,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman) 
            : base(
                schemeParameters, 
                thisPartyKeyingMaterial, 
                fixedInfoFactory, 
                fixedInfoParameter, 
                kdfFactory, 
                kdfParameter, 
                keyConfirmationFactory, 
                keyConfirmationParameter)
        {
            _diffieHellman = diffieHellman;
        }
        
        protected override BitString ComputeSharedSecret(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            return _diffieHellman.GenerateSharedSecretZ(
                (EccDomainParameters) ThisPartyKeyingMaterial.DomainParameters,
                (EccKeyPair) ThisPartyKeyingMaterial.EphemeralKeyPair,
                (EccKeyPair) otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;
        }

        
    }
}