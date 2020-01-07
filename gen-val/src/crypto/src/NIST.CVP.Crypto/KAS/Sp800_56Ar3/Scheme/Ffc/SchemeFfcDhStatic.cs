using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc
{
    internal class SchemeFfcDhStatic : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _diffieHellman;

        public SchemeFfcDhStatic(
            SchemeParameters schemeParameters, 
            ISecretKeyingMaterial thisPartyKeyingMaterial, 
            IFixedInfoFactory fixedInfoFactory, 
            FixedInfoParameter fixedInfoParameter, 
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            MacParameters keyConfirmationParameter,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> diffieHellman) 
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
                (FfcDomainParameters) ThisPartyKeyingMaterial.DomainParameters,
                (FfcKeyPair) ThisPartyKeyingMaterial.StaticKeyPair,
                (FfcKeyPair) otherPartyKeyingMaterial.StaticKeyPair
            ).SharedSecretZ;
        }
    }
}