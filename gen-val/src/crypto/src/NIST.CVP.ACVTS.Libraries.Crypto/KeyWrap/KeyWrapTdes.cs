using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap
{
    public class KeyWrapTdes : KeyWrapBaseTdes
    {
        public static BitString Icv3 = new BitString("A6A6A6A6");
        public KeyWrapTdes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory) : base(engineFactory, cipherFactory) { }

        public override SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher)
        {
            //1.Let ICV3 = 0xA6A6A6A6
            //2.Let S = ICV3 || P
            //3.Return C = TW(S)
            var c = Wrap(key, Icv3.ConcatenateBits(plainText), wrapWithInverseCipher);

            return new SymmetricCipherResult(c);
        }

        public override SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher)
        {
            // 2. Let ICV3 = 0xA6A6A6A6
            // 3. Let S = TW^-1(C)
            var S = WrapInverse(key, cipherText, wrappedWithInverseCipher);

            // 4. If MSB32(S) != ICV3 then return FAIL and stop.
            if (!S.GetMostSignificantBits(32).Equals(Icv3))
            {
                return new SymmetricCipherResult("Authentication failure.");
            }

            // 5. Return P = LSB32(n-1)(S)
            //*authenticated = true;
            var P = S.GetLeastSignificantBits(S.BitLength - 32);
            return new SymmetricCipherResult(P);
        }
    }
}
