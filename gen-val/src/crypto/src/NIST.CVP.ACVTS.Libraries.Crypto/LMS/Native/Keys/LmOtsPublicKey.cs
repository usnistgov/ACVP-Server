using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record LmOtsPublicKey(LmOtsAttribute LmOtsAttribute, byte[] K, byte[] Key) : ILmOtsPublicKey;
}
