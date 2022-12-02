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
    internal class SchemeEccFullUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccFullUnified(
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

            var staticSecret = _diffieHellman.GenerateSharedSecretZ(
                domainParameters,
                (EccKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.StaticKeyPair
            ).SharedSecretZ;

            var ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                domainParameters,
                (EccKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                (EccKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;

            // Shared secret is composed (Z_e || Z_s)
            return BitString.ConcatenateBits(ephemeralSecret, staticSecret);
        }
    }
}
