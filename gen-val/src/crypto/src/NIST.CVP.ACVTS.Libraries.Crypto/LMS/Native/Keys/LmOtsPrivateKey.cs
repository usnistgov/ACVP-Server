using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    internal record LmOtsPrivateKey(LmOtsAttribute LmOtsAttribute, byte[] I, byte[] Q, byte[] Seed, byte[][] X, byte[] Key) : ILmOtsPrivateKey;
}
