using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Class is used to represent contextual information as it pertains to KAS - KDFs, KeyConfirmation, etc.
    /// </summary>
    public class FixedInfoParameter
    {
        /// <summary>
        /// The pattern and order to write the fixed info
        /// </summary>
        public string FixedInfoPattern { get; set; }
        
        /// <summary>
        /// The length of the key that is to be derived or transported.
        /// </summary>
        public int L { get; set; }
        
        /// <summary>
        /// The Salt value used for a MAC operation.
        /// </summary>
        public BitString Salt { get; set; }
        
        /// <summary>
        /// The initialization vector used in some MAC operations.
        /// </summary>
        public BitString Iv { get; set; }
        
        /// <summary>
        /// The encoding type to use on the fixed info data.
        /// </summary>
        public FixedInfoEncoding Encoding { get; set; }
        
        /// <summary>
        /// Party U's fixed info contribution.
        /// </summary>
        public PartyFixedInfo FixedInfoPartyU { get; set; }
        
        /// <summary>
        /// Party V's fixed info contribution.
        /// </summary>
        public PartyFixedInfo FixedInfoPartyV { get; set; }
    }
}