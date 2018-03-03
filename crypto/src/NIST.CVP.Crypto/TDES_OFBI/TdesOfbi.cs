using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFBI
{
    public class TdesOfbi : ITDES_OFBI
    {
        private int BLOCK_SIZE = 64;

        public SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {
            var ivs = SetupIvs(iv);
            var subSegments = plainText.BitLength / BLOCK_SIZE;
            var cipherText = new BitString(0);

            //it's not necessary to keep all the outputs, only the last 3
            //but unless memory consuption becomes an issue, this approach is faster
            var encryptionOutputs = new List<BitString>();

            for (var i = 0; i < subSegments; i++)
            {
                var encryptionInput = i <= 2 ? ivs[i] : encryptionOutputs[i - 3];
                var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));
                encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                var plainTextSegment = plainText.MSBSubstring(i * BLOCK_SIZE, BLOCK_SIZE);
                var cipherTextSegment = plainTextSegment.XOR(encryptionOutput);
                cipherText = cipherText.ConcatenateBits(cipherTextSegment);
            }

            return new SymmetricCipherWithIvResult(cipherText, ivs);

        }


        private static BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new[]{ iv,
                iv.AddWithModulo(new BitString("5555555555555555"), 64),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64)};
        }

        private static byte[] EncryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);
            return output;
        }

        public SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
        {
            var ivs = SetupIvs(iv);
            var subSegments = cipherText.BitLength / BLOCK_SIZE;
            var plainText = new BitString(0);

            //it's not necessary to keep all the outputs, only the last 3
            //but unless memory consuption becomes an issue, this approach is faster
            var encryptionOutputs = new List<BitString>();

            for (var i = 0; i < subSegments; i++)
            {
                var encryptionInput = i <= 2 ? ivs[i] : encryptionOutputs[i - 3];
                var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));
                encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                var cipherTextSegment = cipherText.MSBSubstring(i * BLOCK_SIZE, BLOCK_SIZE);
                var plainTextSegment = cipherTextSegment.XOR(encryptionOutput);
                plainText = plainText.ConcatenateBits(plainTextSegment);
            }

            return new SymmetricCipherWithIvResult(plainText, ivs);
        }
    }
}
