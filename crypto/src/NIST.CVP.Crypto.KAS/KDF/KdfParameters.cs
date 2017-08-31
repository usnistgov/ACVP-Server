namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class KdfParameters
    {
        /// <summary>
        /// Construct KDF parameters
        /// </summary>
        /// <param name="keyLength">The key length</param>
        /// <param name="otherInfoPattern">The other info pattern</param>
        public KdfParameters(int keyLength, string otherInfoPattern)
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