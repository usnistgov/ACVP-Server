using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The "modes" that an <see cref="IKas"/> can run in.
    /// </summary>
    public enum KasMode
    {
        /// <summary>
        /// No Kdf, No Key confirmation (component only test)
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
        KdfKc
    }
}