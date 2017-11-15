using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeParametersFfc : SchemeParametersBase<FfcParameterSet, FfcScheme>
    {
        public SchemeParametersFfc(
            KeyAgreementRole keyAgreementRole, 
            KasMode kasMode, 
            FfcScheme scheme, 
            KeyConfirmationRole keyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection, 
            FfcParameterSet parameterSet, 
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