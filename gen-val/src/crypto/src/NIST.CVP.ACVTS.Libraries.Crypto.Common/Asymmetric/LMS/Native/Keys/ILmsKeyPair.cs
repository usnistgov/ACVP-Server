using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmsKeyPair
    {
        LmsAttribute LmsAttribute { get; }
        LmOtsAttribute LmOtsAttribute { get; }
        ILmsPrivateKey PrivateKey { get; }
        ILmsPublicKey PublicKey { get; }
    }
}
