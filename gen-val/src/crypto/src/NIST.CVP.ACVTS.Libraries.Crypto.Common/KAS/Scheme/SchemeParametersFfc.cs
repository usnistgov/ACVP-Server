using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    public class SchemeParametersFfc : SchemeParametersBase<KasDsaAlgoAttributesFfc>
    {
        public SchemeParametersFfc(
            KasDsaAlgoAttributesFfc kasDsaAlgoAttributes,
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
