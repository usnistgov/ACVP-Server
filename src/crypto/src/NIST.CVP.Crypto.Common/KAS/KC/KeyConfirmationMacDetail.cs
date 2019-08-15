using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KC
{
    /// <summary>
    /// Describes the valid range of values per mac type for use in key confirmation.
    /// </summary>
    public class KeyConfirmationMacDetail
    {
        /// <summary>
        /// The mac type.
        /// </summary>
        public KeyAgreementMacType MacType { get; }
        /// <summary>
        /// The minimum allowed tag len when using a mac for key confirmation purposes.
        /// </summary>
        public int MinTagLen => 64;
        /// <summary>
        /// The maximum allowed tag len when using a mac for key confirmation purposes.
        /// </summary>
        public int MaxTagLen { get; }
        /// <summary>
        /// The minimum key length allowed for the mac.
        /// </summary>
        public int MinKeyLen { get; }
        /// <summary>
        /// The maximum key length allowed for the mac.
        /// </summary>
        public int MaxKeyLen { get; }
        
        public KeyConfirmationMacDetail(KeyAgreementMacType macType, int maxTagLen, int minKeyLen, int maxKeyLen)
        {
            MacType = macType;
            MaxTagLen = maxTagLen;
            MinKeyLen = minKeyLen;
            MaxKeyLen = maxKeyLen;
        }
    }
}