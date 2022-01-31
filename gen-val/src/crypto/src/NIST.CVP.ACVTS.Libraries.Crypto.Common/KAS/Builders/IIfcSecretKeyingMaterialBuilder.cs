using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders
{
    public interface IIfcSecretKeyingMaterialBuilder
    {
        IIfcSecretKeyingMaterialBuilder WithKey(KeyPair value);
        IIfcSecretKeyingMaterialBuilder WithZ(BitString value);
        IIfcSecretKeyingMaterialBuilder WithDkmNonce(BitString value);
        IIfcSecretKeyingMaterialBuilder WithPartyId(BitString value);
        IIfcSecretKeyingMaterialBuilder WithC(BitString value);
        IIfcSecretKeyingMaterialBuilder WithK(BitString value);
        IIfcSecretKeyingMaterial Build(
            IfcScheme scheme,
            KasMode kasMode,
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            bool shouldValidateContributions = true);
    }
}
