using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface ILmOtsKeyPair
    {
        LmOtsAttribute LmOtsAttribute { get; }

        ILmOtsPrivateKey PrivateKey { get; }
        ILmOtsPublicKey PublicKey { get; }
    }
}
