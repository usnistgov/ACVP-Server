using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapAes : KeyWrapBaseAes
    {

        public static BitString Icv1 = new BitString("A6A6A6A6A6A6A6A6");

        public KeyWrapAes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory) : base(engineFactory, cipherFactory) { }

        public override SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher)
        {
            // 1. Let ICV1 = 0xA6A6A6A6A6A6A6A6
            // 2. Let S = ICV1 || P
            // 3. Return C = W(S)
            BitString c = Wrap(key, Icv1.ConcatenateBits(plainText), wrapWithInverseCipher);

            return new SymmetricCipherResult(c);
        }

        public override SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher)
        {
            // 1. Let ICV1 = 0xA6A6A6A6A6A6A6A6
            // 2. Let S = W^-1(C)
            BitString s = WrapInverse(key, cipherText, wrappedWithInverseCipher);
            // 3. If MSB64(S) != ICV1 then return FAIL and stop.
            if (!s.GetMostSignificantBits(64).Equals(Icv1))
            {
                return new SymmetricCipherResult("Authentication failure.");
            }
            
            // 4. Return P = LSB64(n-1)(S)
            BitString p = s.GetLeastSignificantBits((s.BitLength) - 64);
            
            return new SymmetricCipherResult(p);
        }
    }
}
