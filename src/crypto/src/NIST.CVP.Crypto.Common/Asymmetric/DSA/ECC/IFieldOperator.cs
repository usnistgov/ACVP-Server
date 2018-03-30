using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IFieldOperator
    {
        BigInteger Add(BigInteger a, BigInteger b);
        //BigInteger Subtract(BigInteger a, BigInteger b);
        BigInteger Multiply(BigInteger a, BigInteger b);
        BigInteger Divide(BigInteger a, BigInteger b);
        //BigInteger Negate(BigInteger a);
        BigInteger Modulo(BigInteger a);
        BigInteger Inverse(BigInteger a);
    }
}
