using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ecc
{
    public class SchemeParametersEcc : SchemeParametersBase<KasDsaAlgoAttributesEcc>
    {
        public SchemeParametersEcc(
            KasDsaAlgoAttributesEcc kasDsaAlgoAttributes,
            KeyAgreementRole keyAgreementRole,
            KasMode kasMode,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            KasAssurance kasAssurances,
            BitString thisPartyId)
            : base(
                kasDsaAlgoAttributes,
                keyAgreementRole,
                kasMode,
                keyConfirmationRole,
                keyConfirmationDirection,
                kasAssurances,
                thisPartyId
            )
        {
        }
    }
}