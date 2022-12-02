using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc
{
    internal class SchemeFfcDhHybrid1 : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _diffieHellman;

        public SchemeFfcDhHybrid1(
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
            var domainParameters = (FfcDomainParameters)ThisPartyKeyingMaterial.DomainParameters;

            var staticSecret = _diffieHellman.GenerateSharedSecretZ(
                domainParameters,
                (FfcKeyPair)ThisPartyKeyingMaterial.StaticKeyPair,
                (FfcKeyPair)otherPartyKeyingMaterial.StaticKeyPair
            ).SharedSecretZ;

            var ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                domainParameters,
                (FfcKeyPair)ThisPartyKeyingMaterial.EphemeralKeyPair,
                (FfcKeyPair)otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;

            // Shared secret is composed (Z_e || Z_s)
            return BitString.ConcatenateBits(ephemeralSecret, staticSecret);
        }
    }
}
