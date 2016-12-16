using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES
{
    public abstract class Rijndael
    {
        protected readonly IRijndaelInternals _iRijndaelInternals;

        public Rijndael(IRijndaelInternals iRijndaelInternals)
        {
            _iRijndaelInternals = iRijndaelInternals;
        }

        public const int MAXKC = 8;
        public const int MAX_IV_SIZE_BYTES = 16;
        public Key MakeKey(byte[] keyData, DirectionValues direction)
        {
            var key = new Key
            {
                BlockLength = 128,
                Bytes = keyData,
                Direction = direction
            };
            byte[,] k = new byte[4, MAXKC];
            int keyLen = keyData.Length * 8;
            for (int i = 0; i < keyLen / 8; i++)
            {
                k[i % 4, i / 4] = keyData[i];
            }
            key.KeySchedule = new RijndaelKeySchedule(keyData.Length * 8, key.BlockLength, k);
            return key;
        }

        public BitString BlockEncrypt(Cipher cipher, Key key, byte[] plainText, int outputLengthInBits)
        {
            int numBlocks = plainText.Length * 8 / cipher.BlockLength;
            byte[,] block = new byte[4, 8];
            byte[] outBuffer = new byte[outputLengthInBits / 8];

            BlockEncryptWorker(cipher, key, plainText, numBlocks, block, outBuffer);
            return new BitString(outBuffer);
        }

        protected abstract void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block,
            byte[] outBuffer);

        protected void EncryptSingleBlock(byte[,] block, Key key)
        {
            _iRijndaelInternals.EncryptSingleBlock(block, key);
        }

        protected void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount)
        {
            _iRijndaelInternals.KeyAddition(block, roundKey, blockCount);
        }

        protected void Substitution(byte[,] block, byte[] box, int blockCount)
        {
            _iRijndaelInternals.Substitution(block, box, blockCount);
        }

        protected void ShiftRow(byte[,] block, int d, int blockCount)
        {
            _iRijndaelInternals.ShiftRow(block, d, blockCount);
        }

        protected void MixColumn(byte[,] block, int blockCount)
        {
            _iRijndaelInternals.MixColumn(block, blockCount);
        }

        protected byte Multiply(byte a, byte b)
        {
            return _iRijndaelInternals.Multiply(a, b);
        }
    }
}
