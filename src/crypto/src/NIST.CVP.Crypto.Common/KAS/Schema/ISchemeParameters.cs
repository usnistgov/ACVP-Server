using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Schema
{
    /// <summary>
    /// Describes the parameters needed for a KAS scheme
    /// </summary>
    /// <typeparam name="TKasDsaAlgoAttributes"></typeparam>
    public interface ISchemeParameters<out TKasDsaAlgoAttributes>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
    {
        /// <summary>
        /// The attributes specific to either the FFC or ECC scheme
        /// </summary>
        TKasDsaAlgoAttributes KasDsaAlgoAttributes { get; }
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