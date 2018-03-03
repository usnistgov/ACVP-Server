using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class CFBPMode : ICFBPMode
    {
        private int Shift { get; set; }
        public AlgoMode Algo { get; set; }

        public CFBPMode(AlgoMode algo)
        {
            Algo = algo;
            switch (algo)
            {
                case AlgoMode.TDES_CFBP1:
                    Shift = 1;
                    //BlockCipher = new TdesCipher();
                    break;
                case AlgoMode.TDES_CFBP8:
                    Shift = 8;
                    //BlockCipher = new TdesCipher();
                    break;
                case AlgoMode.TDES_CFBP64:
                    Shift = 64;
                    //BlockCipher = new TdesCipher();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, null);
            }
        }

        public SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText, bool includeAuxValues = false)
        {

            if (plainText.BitLength % Shift != 0)
            {
                throw new ArgumentException("Plain text lenght isn't evenly divisble by the segment size");
            }

            var tdesIvs = new BitString[3]; 
            tdesIvs[0] = iv;
            tdesIvs[1] = iv.AddWithModulo(new BitString("5555555555555555"), 64);
            tdesIvs[2] = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64);



            var subSegments = plainText.BitLength / Shift;  

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

            if (includeAuxValues)
            {

                var ct2 = new BitString(EncryptWorker(key, tdesIvs[1].ToBytes())).XOR(plainText).MSBSubstring(0, plainText.BitLength);
                var ct3 = new BitString(EncryptWorker(key, tdesIvs[2].ToBytes())).XOR(plainText).MSBSubstring(0, plainText.BitLength);
                return new SymmetricCipherWithIvResult(null, tdesIvs, new[] { cipherText, ct2, ct3 });
            }
            else
            {
                return new SymmetricCipherWithIvResult(cipherText, tdesIvs);
            }

            


        }

        public SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText1, BitString plainText2, BitString plainText3)
        {
            var result = BlockEncrypt(key, iv, plainText1);
            var encryptionOutput2 = new BitString(EncryptWorker(key, result.IVs[1].ToBytes())).MSBSubstring(0, result.Result.BitLength).XOR(plainText2);
            var encryptionOutput1 = new BitString(EncryptWorker(key, result.IVs[2].ToBytes())).MSBSubstring(0, result.Result.BitLength).XOR(plainText3);
            return new SymmetricCipherWithIvResult(null, result.IVs, new[] { result.Result, encryptionOutput1, encryptionOutput2 });
        }

        public SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText, bool includeAuxValues = false)
        {
            var tdesIvs = new BitString[3]; 
            tdesIvs[0] = iv;
            tdesIvs[1] = iv.AddWithModulo(new BitString("5555555555555555"), 64);
            tdesIvs[2] = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64);

            if (cipherText.BitLength % Shift != 0)
            {
                throw new ArgumentException("Cipher text length isn't evenly divisble by the segment size");
            }

            var subSegments = cipherText.BitLength / Shift;  


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
            if (includeAuxValues)
            {

                var pt2 = new BitString(EncryptWorker(key, tdesIvs[1].ToBytes())).XOR(cipherText).MSBSubstring(0, cipherText.BitLength);
                var pt3 = new BitString(EncryptWorker(key, tdesIvs[2].ToBytes())).XOR(cipherText).MSBSubstring(0, cipherText.BitLength);
                return new SymmetricCipherWithIvResult(null, tdesIvs, new[] { plainText, pt2, pt3 });
            }
            else
            {
                return new SymmetricCipherWithIvResult(plainText, tdesIvs);
            }
            
        }

        public SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText1, BitString cipherText2, BitString cipherText3)
        {
            var result = BlockDecrypt(key, iv, cipherText1);
            var encryptionOutput2 = new BitString(EncryptWorker(key, result.IVs[1].ToBytes())).MSBSubstring(0, result.Result.BitLength).XOR(cipherText2);
            var encryptionOutput1 = new BitString(EncryptWorker(key, result.IVs[2].ToBytes())).MSBSubstring(0, result.Result.BitLength).XOR(cipherText3);
            return new SymmetricCipherWithIvResult(null, result.IVs, new[] { result.Result, encryptionOutput1, encryptionOutput2 });
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

        private byte[] EncryptWorker2(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] output = context.Schedule[1].Apply(interm1);
            return output;
        }

        private byte[] EncryptWorker1(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] output = context.Schedule[0].Apply(input);
            return output;
        }
    }
}
