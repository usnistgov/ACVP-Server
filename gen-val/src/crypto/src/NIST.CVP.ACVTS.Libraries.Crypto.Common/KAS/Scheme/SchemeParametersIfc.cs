using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    public class SchemeParametersIfc : SchemeParametersBase<KasAlgoAttributesIfc>
    {
        public SchemeParametersIfc(
            KasAlgoAttributesIfc kasAlgoAttributes,
            KeyAgreementRole keyAgreementRole,
            KasMode kasMode,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            KasAssurance kasAssurances,
            BitString thisPartyId)
            : base(kasAlgoAttributes, keyAgreementRole, kasMode, keyConfirmationRole, keyConfirmationDirection, kasAssurances, thisPartyId)
        {
        }
    }
}
