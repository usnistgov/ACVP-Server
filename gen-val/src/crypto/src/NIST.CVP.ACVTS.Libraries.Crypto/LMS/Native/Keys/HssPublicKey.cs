using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record HssPublicKey : IHssPublicKey
    {
        public int Levels { get; init; }
        public Task<byte[]> Key { get; init; }
        public Task<byte[][]> Signatures { get; set; }
    }
}
