using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme.Ffc
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

        protected override BitString GetEphemeralDataFromKeyContribution(ISecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole)
        {
            if (secretKeyingMaterial.EphemeralKeyPair != null)
            {
                var ephemKey = (FfcKeyPair) secretKeyingMaterial.EphemeralKeyPair;
                return new BitString(ephemKey.PublicKeyY).PadToModulusMsb(32);
            }

            if (secretKeyingMaterial.EphemeralNonce != null && secretKeyingMaterial.EphemeralNonce?.BitLength != 0)
            {
                return secretKeyingMaterial.EphemeralNonce;
            }

            return secretKeyingMaterial.DkmNonce;
        }
    }
}