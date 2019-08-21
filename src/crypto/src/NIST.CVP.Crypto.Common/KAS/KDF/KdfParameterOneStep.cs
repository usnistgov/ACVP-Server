using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public class KdfParameterOneStep : IKdfParameter
    {
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        public BitString Z { get; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        public int L { get; }
        /// <summary>
        /// The context binding information for the derived key.
        /// </summary>
        public BitString FixedInfo { get; }

        public KdfParameterOneStep(BitString z, int l, BitString fixedInfo)
        {
            Z = z;
            L = l;
            FixedInfo = fixedInfo;
        }
    }
}