using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc
{
    internal abstract class SchemeBaseFfc : SchemeBase
    {
        protected SchemeBaseFfc(
            SchemeParameters schemeParameters,
            ISecretKeyingMaterial thisPartyKeyingMaterial,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters keyConfirmationParameter)
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
        }

        protected override BitString GetEphemeralDataFromKeyContribution(ISecretKeyingMaterial secretKeyingMaterial)
        {
            if (secretKeyingMaterial.EphemeralKeyPair != null)
            {
                var ephemKey = (FfcKeyPair)secretKeyingMaterial.EphemeralKeyPair;

                if (ephemKey.PublicKeyY != 0)
                {
                    return new BitString(ephemKey.PublicKeyY).PadToModulusMsb(32);
                }
            }

            if (secretKeyingMaterial.EphemeralNonce != null && secretKeyingMaterial.EphemeralNonce?.BitLength != 0)
            {
                return secretKeyingMaterial.EphemeralNonce;
            }

            return secretKeyingMaterial.DkmNonce;
        }
    }
}
