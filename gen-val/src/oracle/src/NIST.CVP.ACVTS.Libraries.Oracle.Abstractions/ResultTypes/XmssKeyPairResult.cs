using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class XmssKeyPairResult : IResult
    {
        public BitString Seed { get; set; }
        public IXmssKeyPair KeyPair { get; set; }
    }
}
