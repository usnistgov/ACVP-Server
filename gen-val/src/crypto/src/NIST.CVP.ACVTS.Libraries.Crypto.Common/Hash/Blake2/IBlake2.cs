using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2
{
    public interface IBlake2 : IHash, IMac
    {
        Blake2HashFunction HashFunction { get; }
    }
}
