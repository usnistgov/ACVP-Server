using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX
{
    public class FfxModeBlockCipherParameters : IFfxModeBlockCipherParameters
    {
        public BlockCipherDirections Direction { get; set; }
        public BitString Iv { get; set; }
        public BitString Key { get; set; }
        public BitString Payload { get; set; }
        public bool UseInverseCipherMode => false;
        public int Radix { get; set; }
    }
}
