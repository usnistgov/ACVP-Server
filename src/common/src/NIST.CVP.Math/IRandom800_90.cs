using System.Numerics;

namespace NIST.CVP.Math
{
    public interface IRandom800_90
    {
        BitString GetRandomBitString(int length);
        BitString GetDifferentBitStringOfSameSize(BitString original);
        int GetRandomInt(int minInclusive, int maxExclusive);
        BigInteger GetRandomBigInteger(BigInteger maxInclusive);
        BigInteger GetRandomBigInteger(BigInteger minInclusive, BigInteger maxInclusive);
        string GetRandomAlphaCharacters(int length);
    }
}