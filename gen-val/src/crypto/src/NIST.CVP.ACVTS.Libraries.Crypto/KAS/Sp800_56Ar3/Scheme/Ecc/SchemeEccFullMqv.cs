using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal class SchemeEccFullMqv : SchemeBaseEcc
    {
        private readonly IMqv<EccDomainParameters, EccKeyPair> _mqv;

        public SchemeEccFullMqv(
            SchemeParameters schemeParameters,
            ISecretKeyingMaterial thisPartyKeyingMaterial,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters keyConfirmationParameter,
            IMqv<EccDomainParameters, EccKeyPair> mqv)
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
            _mqv = mqv;
        }

        protected override BitString ComputeSharedSecret(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            return _mqv.GenerateSharedSecretZ(
                (EccDomainParameters)ThisPartyKeyingMaterial.DomainParameters,
                (EccKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                (EccKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;
        }
    }
}
