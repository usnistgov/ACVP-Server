using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IRijndael
    {
        BitString BlockEncrypt(Cipher cipher, Key key, byte[] plainText, int outputLengthInBits);
        Key MakeKey(byte[] keyData, DirectionValues direction, bool useInverseCipher = false);
    }
}