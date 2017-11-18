using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeParametersEcc : SchemeParametersBase<EccParameterSet, EccScheme>
    {
        public SchemeParametersEcc(
            KeyAgreementRole keyAgreementRole,
            KasMode kasMode,
            EccScheme scheme,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            EccParameterSet parameterSet,
            KasAssurance kasAssurances,
            BitString thisPartyId)
            : base(
                keyAgreementRole,
                kasMode,
                scheme,
                keyConfirmationRole,
                keyConfirmationDirection,
                parameterSet,
                kasAssurances,
                thisPartyId
            )
        {
        }
    }
}