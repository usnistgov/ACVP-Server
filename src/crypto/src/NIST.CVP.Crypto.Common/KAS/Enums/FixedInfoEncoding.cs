namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum FixedInfoEncoding
    {
        None,
        /// <summary>
        /// Concatenates all fields making up fixed info together
        /// </summary>
        Concatenation,
        /// <summary>
        /// Concatenates all fields making up fixed info together, using a byte per field to indicate that fields byte length.
        /// </summary>
        ConcatenationWithLengths,
        /// <summary>
        /// Uses ASN.1 encoding as provided by the framework
        /// </summary>
        ASN_1
    }
}