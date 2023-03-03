using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record LmOtsKeyPair : ILmOtsKeyPair
    {
        public LmOtsAttribute LmOtsAttribute { get; init; }
        public ILmOtsPrivateKey PrivateKey { get; init; }
        public ILmOtsPublicKey PublicKey { get; init; }
    }
}
