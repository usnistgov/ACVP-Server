using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record LmsPublicKey(LmsAttribute LmsAttribute, byte[] Key) : ILmsPublicKey
    {
        public LmsPublicKey(byte[] key) : this(AttributesHelper.GetLmsAttribute(AttributesHelper.GetLmsModeFromTypeCode(key.Take(4).ToArray())), key) { }
    }
}
