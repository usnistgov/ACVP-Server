using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
{
    public class ModeBlockCipherParameters : IModeBlockCipherParameters
    {
        public BlockCipherDirections Direction { get; }
        public BitString Iv { get; set; }
        public BitString Key { get; set; }
        public BitString Payload { get; set; }
        public bool UseInverseCipherMode { get; }

        /// <summary>
        /// Construction for modes requiring use of an IV
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="iv"></param>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        /// <param name="useInverseCipherMode"></param>
        public ModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString iv,
            BitString key,
            BitString payload,
            bool useInverseCipherMode = false
        )
            : this(direction, key, payload, useInverseCipherMode)
        {
            Iv = iv;
        }

        /// <summary>
        /// Construction for modes that do not require an IV
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        /// <param name="useInverseCipherMode"></param>
        public ModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString key,
            BitString payload,
            bool useInverseCipherMode = false
        )
        {
            Direction = direction;
            Key = key;
            Payload = payload;
            UseInverseCipherMode = useInverseCipherMode;
        }
    }
}
