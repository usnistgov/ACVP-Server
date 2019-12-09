using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx
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