using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc
{
    internal class SchemeFfcDhOneFlow : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _diffieHellman;

        public SchemeFfcDhOneFlow(
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
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return _diffieHellman.GenerateSharedSecretZ(
                    (FfcDomainParameters)ThisPartyKeyingMaterial.DomainParameters,
                    (FfcKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                    (FfcKeyPair)otherPartyKeyingMaterial.StaticKeyPair
                ).SharedSecretZ;
            }

            return _diffieHellman.GenerateSharedSecretZ(
                (FfcDomainParameters)ThisPartyKeyingMaterial.DomainParameters,
                (FfcKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (FfcKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;
        }
    }
}
