using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The "modes" that an <see cref="IKas"/> can run in.
    /// </summary>
    public enum KasMode
    {
        /// <summary>
        /// No Kdf, No Key confirmation - KAS component only, KTS basic
        /// </summary>
        [EnumMember(Value = "noKdfNoKc")]
        NoKdfNoKc,
        /// <summary>
        /// No Key Confirmation test - uses a MAC function and KDF on the generated secret
        /// Kdf, No Key Confirmation
        /// </summary>
        [EnumMember(Value = "kdfNoKc")]
        KdfNoKc,
        /// <summary>
        /// Key Confirmation test - uses a MAC function, KDF, and Key Confirmation
        /// Kdf, Key Confirmation
        /// </summary>
        [EnumMember(Value = "kdfKc")]
        KdfKc,
        /// <summary>
        /// No Kdf with KeyConfirmation - utilized in KTS schemes with key confirmation.
        /// </summary>
        [EnumMember(Value = "noKdfKc")]
        NoKdfKc
    }
}
