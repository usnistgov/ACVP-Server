using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2
{
    public interface IBlake2
    {
        Blake2HashFunction HashFunction { get; }

        HashResult HashMessage(BitString message, BitString key = null);
    }
}
