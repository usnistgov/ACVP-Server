using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Linq;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class CFBPMode : ConfidentialityMode
    {
        private int Shift { get; set; }
        public CFBPMode(Algo algo)
        {
            Algo = algo;
            switch (algo)
            {
                case Algo.TDES_CFBP1:
                    Shift = 1;
                    //BlockCipher = new TdesCipher();
                    break;
                case Algo.TDES_CFBP8:
                    Shift = 8;
                    //BlockCipher = new TdesCipher();
                    break;
                case Algo.TDES_CFBP64:
                    Shift = 64;
                    //BlockCipher = new TdesCipher();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, null);
            }
        }
        //Input: P1, P2, …, Pn; IV1, IV2, IV3. |Pi| = k, |IVj| = 64.
        public override EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText)
        {

            var tdesIvs = new BitString[3]; //TODO is this the best representation?
            tdesIvs[0] = iv;
            tdesIvs[1] = iv.AddWithModulo(new BitString("5555555555555555"), 64);
            tdesIvs[2] = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64);

            var subSegments = plainText.BitLength / Shift;  //todo check mod and reject invalid input

            var cipherText = new BitString(0);

            var previousEncryptionInput = new BitString(0);

            for (var i = 0; i < subSegments; i++)
            {
                BitString encryptionInput;
                if (i <= 2)
                {
                    encryptionInput = tdesIvs[i];
                }
                else
                {
                    encryptionInput = previousEncryptionInput
                            .MSBSubstring(Shift, 64 - Shift)
                            .ConcatenateBits(cipherText.MSBSubstring((i - 3) * Shift, Shift))
                        ; 
                }

                var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));
                previousEncryptionInput = encryptionInput.GetDeepCopy();

                var encryptionOutputSegment = encryptionOutput.MSBSubstring(0, Shift);
                var plainTextSegment = plainText.MSBSubstring(i * Shift, Shift);
                var cipherTextSegment = plainTextSegment.XOR(encryptionOutputSegment);
                cipherText = cipherText.ConcatenateBits(cipherTextSegment);
            }



            return new EncryptionResultWithIv(cipherText, tdesIvs);
        }

        public override DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText)
        {
            var tdesIvs = new BitString[3]; //TODO is this the best representation?
            tdesIvs[0] = iv;
            tdesIvs[1] = iv.AddWithModulo(new BitString("5555555555555555"), 64);
            tdesIvs[2] = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64);

            var subSegments = cipherText.BitLength / Shift;  //todo check mod and reject invalid input

            var plainText = new BitString(0);

            var previousEncryptionInput = new BitString(0);

            for (var i = 0; i < subSegments; i++)
            {
                BitString encryptionInput;
                if (i <= 2)
                {
                    encryptionInput = tdesIvs[i];
                }
                else
                {
                    encryptionInput = previousEncryptionInput
                            .MSBSubstring(Shift, 64 - Shift)
                            .ConcatenateBits(cipherText.MSBSubstring((i - 3) * Shift, Shift))
                        ;
                }

                var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));
                previousEncryptionInput = encryptionInput.GetDeepCopy();

                var encryptionOutputSegment = encryptionOutput.MSBSubstring(0, Shift);
                var cipherTextSegment = cipherText.MSBSubstring(i * Shift, Shift);
                var plainTextSegment = cipherTextSegment.XOR(encryptionOutputSegment);
                plainText = plainText.ConcatenateBits(plainTextSegment);
            }

            return new DecryptionResultWithIv(plainText, tdesIvs);
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
