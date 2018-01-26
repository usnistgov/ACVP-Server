using System;
using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelTest : Rijndael
    {
        public RijndaelTest(IRijndaelInternals iRijndaelInternals)
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            throw new NotImplementedException("This class is purely here as a testing facade for the underlying Rijndael implementation methods. It should not be used for encryption");
        }

        public new void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount)
        {
            base.KeyAddition(block, roundKey, blockCount);
        }

        public new void Substitution(byte[,] block, byte[] box, int blockCount)
        {
            base.Substitution(block, box, blockCount);
        }

        public new void ShiftRow(byte[,] block, int d, int blockCount)
        {
            base.ShiftRow(block, d, blockCount);
        }

        public new void MixColumn(byte[,] block, int blockCount)
        {
            base.MixColumn(block, blockCount);
        }

        public new byte Multiply(byte a, byte b)
        {
            return base.Multiply(a, b);
        }
    }
}
