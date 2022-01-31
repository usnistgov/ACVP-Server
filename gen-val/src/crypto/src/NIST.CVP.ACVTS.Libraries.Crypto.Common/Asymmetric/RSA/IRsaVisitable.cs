using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
{
    public interface IRsaVisitable
    {
        BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey);
    }
}
