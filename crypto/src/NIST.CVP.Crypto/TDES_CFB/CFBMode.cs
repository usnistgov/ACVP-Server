using System;
using System.Collections;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class CFBMode : ICFBMode
    {
        private int Shift { get; set; }
        public AlgoMode Algo { get; set;}
        public CFBMode(AlgoMode algo)
        {
            Algo = algo;
            switch (algo)
            {
                case AlgoMode.TDES_CFB1:
                    Shift = 1;
                    //BlockCipher = new TdesCipher();
                    break;
                case AlgoMode.TDES_CFB8:
                    Shift = 8;
                    //BlockCipher = new TdesCipher();
                    break;
                case AlgoMode.TDES_CFB64:
                    Shift = 64;
                    //BlockCipher = new TdesCipher();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, null);
            }
        }

        public SymmetricCipherResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {
            if (plainText.BitLength % Shift != 0)
            {
                throw new ArgumentException("Plain text lenght isn't evenly divisble by the segment size");
            }

            var output = new bool[plainText.BitLength];
            var subSegments = plainText.BitLength / Shift;  
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
            return new SymmetricCipherResult(new BitString(new BitArray(output.ToArray())));
        }
        public SymmetricCipherResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
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
            return new SymmetricCipherResult(new BitString(new BitArray(output.ToArray())));
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
