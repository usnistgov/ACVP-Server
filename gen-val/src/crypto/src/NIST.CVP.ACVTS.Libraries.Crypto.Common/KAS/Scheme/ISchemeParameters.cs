﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    /// <summary>
    /// Describes the parameters needed for a KAS scheme
    /// </summary>
    /// <typeparam name="TKasAlgoAttributes"></typeparam>
    public interface ISchemeParameters<out TKasAlgoAttributes>
        where TKasAlgoAttributes : IKasAlgoAttributes
    {
        /// <summary>
        /// The attributes specific to the KAS scheme
        /// </summary>
        TKasAlgoAttributes KasAlgoAttributes { get; }
        /// <summary>
        /// The Assurances that are implemented by the KAS instance.
        /// </summary>
        KasAssurance KasAssurances { get; }
        /// <summary>
        /// The mode of the KAS attempt
        /// </summary>
        KasMode KasMode { get; }
        /// <summary>
        /// This party's key agreement role
        /// </summary>
        KeyAgreementRole KeyAgreementRole { get; }
        /// <summary>
        /// This party's key confirmation direction
        /// </summary>
        KeyConfirmationDirection KeyConfirmationDirection { get; }
        /// <summary>
        /// This party's key confirmation role
        /// </summary>
        KeyConfirmationRole KeyConfirmationRole { get; }
        /// <summary>
        /// The ID associated with this party
        /// </summary>
        BitString ThisPartyId { get; }
    }
}
