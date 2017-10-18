using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace NIST.CVP.Crypto.Core
{
    public class OFBMode : ConfidentialityMode
    {
        public int Shift { get; set; }
        public OFBMode(BlockCipher cipher, int shift) : base(cipher)
        {
            Shift = shift;
        }

        public override EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {
            var output = new bool[plainText.BitLength];
            
            var subSegments = plainText.BitLength / Shift;  //to do this will break for non evenly divisble ones
            BitString previousEncryptionInput = null;
            BitString previousCipherSegment = null;
            for (int i = 0; i < subSegments; i++)
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

                //due to LSB vs RSB differences, i had to load the array in an odd way.
                //there must be a better way that I am not seeing
                for (var bitIndex = 0; bitIndex < previousCipherSegment.BitLength; bitIndex++)
                {
                    output[i * Shift + bitIndex] = Convert.ToBoolean(previousCipherSegment.Bits[previousCipherSegment.BitLength - 1 - bitIndex]);
                }

            }
            return new EncryptionResult(new BitString(new BitArray(output.Reverse().ToArray())));
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

        public override DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
        {
            var output = new bool[cipherText.BitLength];

            var subSegments = cipherText.BitLength / Shift;  //to do this will break for non evenly divisble ones
            BitString previousEncryptionInput = null;
            BitString previousCtSegment = null;
            for (int i = 0; i < subSegments; i++)
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
                Debug.WriteLine($"Encryption: Input {encryptionInput.ToHex()}  Output {new BitString(encryptionOutput).ToHex()}");
                var ctSegment = cipherText.MSBSubstring(i * Shift, Shift);
                previousCtSegment = ctSegment.GetDeepCopy();

                var encryptionOutputSegment = new BitString(encryptionOutput).GetMostSignificantBits(Shift);
                var plainTextSegment = ctSegment.XOR(encryptionOutputSegment);


                //due to LSB vs RSB differences, i had to load the array in an odd way.
                //there must be a better way that I am not seeing
                for (var bitIndex = 0; bitIndex < plainTextSegment.BitLength; bitIndex++)
                {
                    output[i * Shift + bitIndex] = Convert.ToBoolean(plainTextSegment.Bits[plainTextSegment.BitLength - 1 - bitIndex]);
                }

            }
            return new DecryptionResult(new BitString(new BitArray(output.Reverse().ToArray())));
        }




    }
}
