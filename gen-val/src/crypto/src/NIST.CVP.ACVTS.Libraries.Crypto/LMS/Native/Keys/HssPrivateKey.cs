using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record HssPrivateKey : IHssPrivateKey
    {
        public int Levels { get; init; }
        public Task<ILmsKeyPair[]> Keys { get; init; }
    }
}
