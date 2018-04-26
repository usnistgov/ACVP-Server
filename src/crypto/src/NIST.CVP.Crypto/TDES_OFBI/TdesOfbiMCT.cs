using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;

namespace NIST.CVP.Crypto.TDES_OFBI
{
    public class TdesOfbiMCT : ITDES_OFBI_MCT
    {
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        //private const int NUMBER_OF_CASES = 5;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public TdesOfbiMCT(IMonteCarloKeyMaker keyMaker)
        {

            _keyMaker = keyMaker;
        }

        public MCTResult<AlgoArrayResponseWithIvs> MCTEncrypt(BitString keyBits, BitString iv, BitString data)
        {
            var ivs = SetupIvs(iv);

            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = keyBits,
                    PlainText = data
                }
            };
            var lastCipherTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                var encryptionOutputs = new List<BitString>();
                var cipherText = new BitString(0);
                var encryptionInput = new BitString(0);
                var key = responses.Last().Keys.GetDeepCopy();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    encryptionInput = j <= 2 ? ivs[j] : encryptionOutputs[j - 3];
                    var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));

                    encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                    cipherText = data.XOR(encryptionOutput);
                    //if (j < 5 || j > 9994)
                    //{
                    //    Debug.WriteLine($"J: {j}");
                    //    Debug.WriteLine($"   INPUT {encryptionInput.ToHex()}");
                    //    Debug.WriteLine($"   OUTPUT {encryptionOutput.ToHex()}");
                    //    Debug.WriteLine($"   CIPHER {cipherText.ToHex()}");
                    //}
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, cipherText.GetDeepCopy());
                    }
                    data = encryptionInput.GetDeepCopy();
                }

                responses.Last().CipherText = cipherText.GetDeepCopy();

                ivs = SetupIvs(encryptionOutputs[9995].XOR(cipherText));

                responses.Add(new AlgoArrayResponseWithIvs
                {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList()).ToOddParityBitString(),
                    PlainText =  responses.Last().PlainText.XOR(encryptionInput)
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
        }

        public MCTResult<AlgoArrayResponseWithIvs> MCTDecrypt(BitString keyBits, BitString iv, BitString data)
        {
            var ivs = SetupIvs(iv);

            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = keyBits,
                    CipherText = data
                }
            };
            var lastPlainTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                var encryptionOutputs = new List<BitString>();
                var plainText = new BitString(0);
                var encryptionInput = new BitString(0);
                var key = responses.Last().Keys.GetDeepCopy();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    encryptionInput = j <= 2 ? ivs[j] : encryptionOutputs[j - 3];
                    var encryptionOutput = new BitString(EncryptWorker(key, encryptionInput.ToBytes()));

                    encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                    plainText = data.XOR(encryptionOutput);
                    //if (j < 5 || j > 9994)
                    //{
                    //    Debug.WriteLine($"J: {j}");
                    //    Debug.WriteLine($"   INPUT {encryptionInput.ToHex()}");
                    //    Debug.WriteLine($"   OUTPUT {encryptionOutput.ToHex()}");
                    //    Debug.WriteLine($"   PLAIN {plainText.ToHex()}");
                    //}
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, plainText.GetDeepCopy());
                    }
                    data = encryptionInput.GetDeepCopy();
                }

                responses.Last().PlainText = plainText.GetDeepCopy();

                ivs = SetupIvs(encryptionOutputs[9995].XOR(plainText));

                responses.Add(new AlgoArrayResponseWithIvs
                {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList()).ToOddParityBitString(),
                    CipherText = responses.Last().CipherText.XOR(encryptionInput)
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
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

        private BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new[]{ iv,
                iv.AddWithModulo(new BitString("5555555555555555"), 64),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64)};
        }
    }
}
