using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The role of the entity involved in the Key Agreement
    /// </summary>
    public enum KeyAgreementRole
    {
        /// <summary>
        /// No Key Agreement Role was set (invalid)
        /// </summary>
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// The initiator, aka party U
        /// </summary>
        [EnumMember(Value = "initiator")]
        InitiatorPartyU,
        /// <summary>
        /// The responder, aka party V
        /// </summary>
        [EnumMember(Value = "responder")]
        ResponderPartyV
    }
}
