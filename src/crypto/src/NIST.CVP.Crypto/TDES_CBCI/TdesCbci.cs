using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;

namespace NIST.CVP.Crypto.TDES_CBCI
{
    public class TdesCbci : ITDES_CBCI
    {
        private const int BLOCK_SIZE = 64;
        private const int PARTITIONS = 3;



        public SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {
            if (plainText.BitLength / BLOCK_SIZE < PARTITIONS)
            {
                throw new ArgumentException($"CBCI mode needs at least {PARTITIONS} blocks of data");
            }

            var plainTexts = TriPartitionBitString(plainText);
            var ivs = SetupIvs(iv);
            var cipherText = new byte[plainText.BitLength / 8];

            for (var i = 0; i < PARTITIONS; i++)
            {
                var currentIv = ivs[i];
                var numOfBlocks = plainTexts[i].BitLength / BLOCK_SIZE;
                for (var j = 0; j < numOfBlocks; j++)
                {
                    var ptSegment = plainTexts[i].MSBSubstring(j * BLOCK_SIZE, BLOCK_SIZE);
                    var encInput = ptSegment.XOR(currentIv);
                    var output = EncryptWorker(key, encInput.ToBytes());
                    var ctIndex = (j * 3) + i;
                    output.CopyTo(cipherText, ctIndex * BLOCK_SIZE / 8);
                    currentIv = new BitString(output).GetDeepCopy();
                }
            }

            return new SymmetricCipherWithIvResult(new BitString(cipherText), ivs);

        }


        public SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
        {
            if (cipherText.BitLength / BLOCK_SIZE < PARTITIONS)
            {
                throw new ArgumentException($"CBCI mode needs at least {PARTITIONS} blocks of data");
            }

            var cipherTexts = TriPartitionBitString(cipherText);
            var ivs = SetupIvs(iv);
            var plainText = new byte[cipherText.BitLength / 8];

            for (var i = 0; i < PARTITIONS; i++)
            {
                var prevEncInput = ivs[i];
                var numOfBlocks = cipherTexts[i].BitLength / BLOCK_SIZE;
                for (var j = 0; j < numOfBlocks; j++)
                {
                    var ctSegment = cipherTexts[i].MSBSubstring(j * BLOCK_SIZE, BLOCK_SIZE);
                    var output = new BitString(DecryptWorker(key, ctSegment.ToBytes()));
                    var ptIndex = (j * 3) + i;
                    output.XOR(prevEncInput).ToBytes().CopyTo(plainText, ptIndex * BLOCK_SIZE / 8);
                    prevEncInput = ctSegment.GetDeepCopy();
                }
            }


            return new SymmetricCipherWithIvResult(new BitString(plainText), ivs);
            
        }

        private BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new []{ iv,
                iv.AddWithModulo(new BitString("5555555555555555"), 64),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64)};
        }

        private BitString[] TriPartitionBitString(BitString bitString)
        {
            //string needs to be evently splittable into three parts, and be on the byte boundary. 3 * 8 = 24
            if (bitString.BitLength % 24 != 0) 
            {
                throw new Exception($"Can't tripartition a bitstring of size {bitString.BitLength}");
            }

            /*
            //keeping this here for reference, but it doesn't seem to be any faster
            var inputBytes = bitString.ToBytes();

            var substringSizeInBytes = bitString.BitLength / 24;
            var output = new List<byte[]> { new byte[substringSizeInBytes], new byte[substringSizeInBytes], new byte[substringSizeInBytes] };


            for (var i = 0; i < bitString.BitLength / BLOCK_SIZE; i++)
            {
                var ptIndex = i % PARTITIONS;
                Array.Copy(inputBytes, i * 8, output[ptIndex], (i / 3) * 8, 8);
            }

            var retVal = new BitString[PARTITIONS];
            for (var i = 0; i < PARTITIONS; i++)
            {
                retVal[i] = new BitString(output[i]);
            }

            return retVal;
            */
            
            var retVal = new BitString[PARTITIONS];
            for (var i = 0; i < PARTITIONS; i++)
            {
                retVal[i] = new BitString(0);
            }

            for (var i = 0; i < bitString.BitLength / BLOCK_SIZE; i++)
            {
                var ptIndex = i % PARTITIONS;
                retVal[ptIndex] = retVal[ptIndex].ConcatenateBits(bitString.MSBSubstring(i * BLOCK_SIZE, BLOCK_SIZE));
            }
            return retVal;
            
            

        }


        private byte[] EncryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);
            return output;
        }

        private byte[] DecryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Decryption);
            byte[] interm1 = context.Schedule[2].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[0].Apply(interm2);
            return output;
        }


    }
}
