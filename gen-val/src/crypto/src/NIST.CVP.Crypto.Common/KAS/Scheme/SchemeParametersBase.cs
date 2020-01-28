using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public abstract class SchemeParametersBase<TKasAlgoAttributes> : ISchemeParameters<TKasAlgoAttributes>
        where TKasAlgoAttributes : IKasAlgoAttributes
    {
        /// <summary>
        /// Constructs Kas parameter information
        /// </summary>
        /// <param name="keyAgreementRole">This party's key agreement role</param>
        /// <param name="kasMode">The mode of the KAS attempt</param>
        /// <param name="keyConfirmationRole">This party's key confirmation role</param>
        /// <param name="keyConfirmationDirection">This party's key confirmation direction</param>
        /// <param name="kasAssurances">The assurances associated with the KAS</param>
        /// <param name="thisPartyId">The ID associated with this party</param>
        protected SchemeParametersBase(
            TKasAlgoAttributes kasAlgoAttributes,
            KeyAgreementRole keyAgreementRole, 
            KasMode kasMode,
            KeyConfirmationRole keyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection,
            KasAssurance kasAssurances,
            BitString thisPartyId
        )
        {
            if (kasMode != KasMode.NoKdfNoKc && BitString.IsZeroLengthOrNull(thisPartyId))
            {
                throw new ArgumentException(nameof(thisPartyId));
            }

            if (kasMode == KasMode.KdfKc)
            {
                if (keyConfirmationRole == KeyConfirmationRole.None ||
                    keyConfirmationDirection == KeyConfirmationDirection.None)
                {
                    throw new ArgumentException(
                        $"{nameof(KasMode.KdfKc)} requires a valid (not None) value for both {nameof(keyConfirmationRole)} and {nameof(keyConfirmationDirection)}");
                }
            }
            KasAlgoAttributes = kasAlgoAttributes;
            KeyAgreementRole = keyAgreementRole;
            KasMode = kasMode;
            KeyConfirmationRole = keyConfirmationRole;
            KeyConfirmationDirection = keyConfirmationDirection;
            KasAssurances = kasAssurances;
            ThisPartyId = thisPartyId;
        }
        /// <inheritdoc />
        public TKasAlgoAttributes KasAlgoAttributes { get; }
        /// <inheritdoc />
        public KeyAgreementRole KeyAgreementRole { get; }
        /// <inheritdoc />
        public KasMode KasMode { get; }
        /// <inheritdoc />
        public KeyConfirmationRole KeyConfirmationRole { get; }
        /// <inheritdoc />
        public KeyConfirmationDirection KeyConfirmationDirection { get; }
        /// <inheritdoc />
        public KasAssurance KasAssurances { get; }
        /// <inheritdoc />
        public BitString ThisPartyId { get; }
    }
}