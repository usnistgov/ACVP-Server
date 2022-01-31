using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS
{
    public class XtsModeBlockCipherParameters : IXtsModeBlockCipherParameters
    {
        public BlockCipherDirections Direction { get; }
        public BitString Key { get; set; }
        public BitString Payload { get; set; }
        public int DataUnitLength { get; }
        public BitString Tweak { get; set; }

        /// <summary>
        /// Do not use
        /// </summary>
        public BitString Iv
        {
            get => null;
            set { }
        } // Not used

        /// <summary>
        /// Do not use
        /// </summary>
        public bool UseInverseCipherMode { get; }

        public XtsModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString tweak,
            BitString key,
            BitString payload,
            int dataUnitLength)
        {
            Direction = direction;
            Tweak = tweak;
            Key = key;
            Payload = payload;
            DataUnitLength = dataUnitLength;
        }
    }
}
