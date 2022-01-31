using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum FixedInfoEncoding
    {
        None,
        /// <summary>
        /// Concatenates all fields making up fixed info together
        /// </summary>
        [EnumMember(Value = "concatenation")]
        Concatenation,
        /// <summary>
        /// Concatenates all fields making up fixed info together, using a byte per field to indicate that fields byte length.
        /// </summary>
        [EnumMember(Value = "concatenationWithLengths")]
        ConcatenationWithLengths,
        /// <summary>
        /// Uses ASN.1 encoding as provided by the framework
        /// </summary>
        [EnumMember(Value = "asn.1")]
        ASN_1
    }
}
