using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record LmsKeyPair : ILmsKeyPair
    {
        public LmsAttribute LmsAttribute { get; init; }
        public LmOtsAttribute LmOtsAttribute { get; init; }
        public ILmsPrivateKey PrivateKey { get; init; }
        public ILmsPublicKey PublicKey { get; init; }
    }
}
