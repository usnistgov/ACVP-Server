using System;
using System.Collections;
using System.Linq;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class CFBMode : ConfidentialityMode
    {
        private int Shift { get; set; }
        public CFBMode(Algo algo)
        {
            Algo = algo;
            switch (algo)
            {
                case Algo.TDES_CFB1:
                    Shift = 1;
                    //BlockCipher = new TdesCipher();
                    break;
                case Algo.TDES_CFB8:
                    Shift = 8;
                    //BlockCipher = new TdesCipher();
                    break;
                case Algo.TDES_CFB64:
                    Shift = 64;
                    //BlockCipher = new TdesCipher();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, null);
            }
        }

        public override EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {
            
            var output = new bool[plainText.BitLength];
            var subSegments = plainText.BitLength / Shift;  //todo check mod and reject invalid input
            BitString previousEncryptionInput = null;
            BitString previousCipherSegment = null;
            for (var i = 0; i < subSegments; i++)
            {
                BitString encryptionInput;
                if (i == 0)
                {
                    encryptionInput = iv.GetDeepCopy();
                }
                else
                {
                    encryptionInput = previousEncryptionInput
                        .GetLeastSignificantBits(previousEncryptionInput.BitLength - Shift)
                        .ConcatenateBits(previousCipherSegment);
                }
                previousEncryptionInput = encryptionInput.GetDeepCopy();
                var encryptionOutput = EncryptWorker(key, encryptionInput.ToBytes());
                var ptSegment = plainText.MSBSubstring(i * Shift, Shift);
                var encryptionOutputSegment = new BitString(encryptionOutput).GetMostSignificantBits(Shift);
                var cipherTextSegment = ptSegment.XOR(encryptionOutputSegment);
                previousCipherSegment = cipherTextSegment.GetDeepCopy();
                for (var bitIndex = 0; bitIndex < previousCipherSegment.BitLength; bitIndex++)
                {
                    output[(subSegments - i) * Shift - bitIndex - 1] = Convert.ToBoolean(previousCipherSegment.Bits[Shift - 1 - bitIndex]);
                }
            }
            return new EncryptionResult(new BitString(new BitArray(output.ToArray())));
        }
        public override DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
        {
            
            var output = new bool[cipherText.BitLength];
            var subSegments = cipherText.BitLength / Shift;  //to do this will break for non evenly divisble ones
            BitString previousEncryptionInput = null;
            BitString previousCtSegment = null;
            for (var i = 0; i < subSegments; i++)
            {
                
                BitString encryptionInput;
                if (i == 0)
                {
                    encryptionInput = iv.GetDeepCopy();
                }
                else
                {
                    encryptionInput = previousEncryptionInput
                        .GetLeastSignificantBits(previousEncryptionInput.BitLength - Shift)
                        .ConcatenateBits(previousCtSegment);
                }
                
                previousEncryptionInput = encryptionInput.GetDeepCopy();
                var encryptionOutput = EncryptWorker(key, encryptionInput.ToBytes());

                var ctSegment = cipherText.MSBSubstring(i * Shift, Shift);
                previousCtSegment = ctSegment.GetDeepCopy();
                var encryptionOutputSegment = new BitString(encryptionOutput).GetMostSignificantBits(Shift);
                var plainTextSegment = ctSegment.XOR(encryptionOutputSegment);
                
                for (var bitIndex = 0; bitIndex < Shift; bitIndex++)
                {
                    output[(subSegments - i) * Shift - bitIndex - 1] = Convert.ToBoolean(plainTextSegment.Bits[Shift - 1 - bitIndex]);
                }
            }
            return new DecryptionResult(new BitString(new BitArray(output.ToArray())));
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




    }
}
