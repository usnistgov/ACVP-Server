using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public class KasParameters
    {
        /// <summary>
        /// Constructs Kas parameter information
        /// </summary>
        /// <param name="keyAgreementRole">This party's key agreement role</param>
        /// <param name="keyConfirmationRole">This party's key confirmation role</param>
        /// <param name="keyConfirmationDirection">This party's key confirmation direction</param>
        /// <param name="ffcParameterSet">The parameter set used in the KAS operation</param>
        /// <param name="kasAssurances">The assuances associated with the KAS</param>
        /// <param name="thisPartyId">The ID associated with this party</param>
        public KasParameters(
            KeyAgreementRole keyAgreementRole, 
            KeyConfirmationRole keyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection, 
            FfcParameterSet ffcParameterSet,
            KasAssurance kasAssurances,
            BitString thisPartyId
        )
        {
            KeyAgreementRole = keyAgreementRole;
            KeyConfirmationRole = keyConfirmationRole;
            KeyConfirmationDirection = keyConfirmationDirection;
            FfcParameterSet = ffcParameterSet;
            KasAssurances = kasAssurances;
            ThisPartyId = thisPartyId;
        }

        /// <summary>
        /// This party's key agreement role
        /// </summary>
        public KeyAgreementRole KeyAgreementRole { get; }
        /// <summary>
        /// This party's key confirmation role
        /// </summary>
        public KeyConfirmationRole KeyConfirmationRole { get; }
        /// <summary>
        /// This party's key confirmation direction
        /// </summary>
        public KeyConfirmationDirection KeyConfirmationDirection { get; }
        /// <summary>
        /// The parameter set utilized in the KAS operation
        /// </summary>
        public FfcParameterSet FfcParameterSet { get; }
        /// <summary>
        /// The Assurances that are implemented by the KAS instance.
        /// </summary>
        public KasAssurance KasAssurances { get; }
        /// <summary>
        /// The ID associated with this party
        /// </summary>
        public BitString ThisPartyId { get; }
    }
}