using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal class SchemeEccOnePassUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccOnePassUnified(
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
            var domainParameters = (EccDomainParameters)ThisPartyKeyingMaterial.DomainParameters;

            // static secret composed of both parties static key parts
            var staticSecret = _diffieHellman.GenerateSharedSecretZ(
                domainParameters,
                (EccKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.StaticKeyPair
            ).SharedSecretZ;

            // differing ephemeralSecret logic depending on key agreement role
            BitString ephemeralSecret;
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                    domainParameters,
                    (EccKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                    (EccKeyPair)otherPartyKeyingMaterial.StaticKeyPair
                ).SharedSecretZ;
            }
            else
            {
                ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                    domainParameters,
                    (EccKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                    (EccKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
                ).SharedSecretZ;
            }

            // Shared secret is composed (Z_e || Z_s)
            return BitString.ConcatenateBits(ephemeralSecret, staticSecret);
        }
    }
}
