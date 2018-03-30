namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public class KdfParameters
    {
        /// <summary>
        /// Construct KDF parameters
        /// </summary>
        /// <param name="keyLength">The key length</param>
        /// <param name="otherInfoPattern">The other info pattern.  CAVS default pattern of "uPartyInfo||vPartyInfo"</param>
        public KdfParameters(int keyLength, string otherInfoPattern = "uPartyInfo||vPartyInfo")
        {
            KeyLength = keyLength;
            OtherInfoPattern = otherInfoPattern;
        }

        /// <summary>
        /// The length of the key to generate
        /// </summary>
        public int KeyLength { get; }
        /// <summary>
        /// Pattern used for constructing other info
        /// </summary>
        public string OtherInfoPattern { get; }
    }
}