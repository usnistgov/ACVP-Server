using System.ComponentModel;

namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The role of the entity involved in the Key Agreement
    /// </summary>
    public enum KeyAgreementRole
    {
        /// <summary>
        /// The initiator, aka party U
        /// </summary>
        [Description("initiator")]
        InitiatorPartyU,
        /// <summary>
        /// The responder, aka party V
        /// </summary>
        [Description("responder")]
        ResponderPartyV
    }
}