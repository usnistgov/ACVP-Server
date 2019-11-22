using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3
{
    public class SchemeParameters : SchemeParametersBase<KasAlgoAttributes>
    {
        public SchemeParameters(
            KasAlgoAttributes kasAlgoAttributes, 
            KeyAgreementRole keyAgreementRole, 
            KasMode kasMode, 
            KeyConfirmationRole keyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection, 
            KasAssurance kasAssurances, 
            BitString thisPartyId
            ) : base(kasAlgoAttributes, keyAgreementRole, kasMode, keyConfirmationRole, keyConfirmationDirection, kasAssurances, thisPartyId)
        {
        }
    }
}