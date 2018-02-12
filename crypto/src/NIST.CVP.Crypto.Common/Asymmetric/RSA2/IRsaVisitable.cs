using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public interface IRsaVisitable
    {
        BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey);
    }
}
