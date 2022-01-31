using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS
{
    public interface IXtsModeBlockCipherParameters : IModeBlockCipherParameters
    {
        int DataUnitLength { get; }

        BitString Tweak { get; set; }
    }
}
