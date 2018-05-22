using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Crypto.TDES_OFB
{
    public class TDES_OFB_MCT : ITDES_OFB_MCT
    {
        private readonly ITDES_OFB _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public TDES_OFB_MCT(ITDES_OFB algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _keyMaker = keyMaker;
        }

        public Common.Symmetric.TDES.MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv)
        {
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = iv,
                    Keys = keyBits,
                    PlainText = data
                }
            };

            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {

                var tempText = responses.Last().PlainText.GetDeepCopy();
                var tempIv = responses.Last().IV.GetDeepCopy();
                BitString prevTempIv = null;
                SymmetricCipherResult encryptionResult = null;

                var lastCipherTexts = new List<BitString>();
                BitString output = null;
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    prevTempIv = tempIv;
                    encryptionResult = _algo.BlockEncrypt(responses[i].Keys, tempText, tempIv);
                    //the result is OUTPUT xored with TEXT. 
                    //Output is needed for the next step, so let's recompute it
                    output = encryptionResult.Result.XOR(tempText);

                    tempText = tempIv.GetDeepCopy();
                    tempIv = output.GetDeepCopy();
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, encryptionResult.Result.GetDeepCopy());
                    }

                }
                responses.Last().CipherText = encryptionResult.Result.GetDeepCopy();

                responses.Add(new AlgoArrayResponse()
                    {
                        Keys = _keyMaker.MixKeys(new TDESKeys(responses.Last().Keys.GetDeepCopy()), lastCipherTexts).ToOddParityBitString(),
                        PlainText = responses.Last().PlainText.GetDeepCopy().XOR(prevTempIv),
                        IV = output.GetDeepCopy()
                });

            }
            responses.RemoveAt(responses.Count() - 1);
            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(responses);

        }

        public Common.Symmetric.TDES.MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv)
        {
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = iv,
                    Keys = keyBits,
                    CipherText = data
                }
            };

            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {

                var tempCipherText = responses.Last().CipherText.GetDeepCopy();
                var tempIv = responses.Last().IV.GetDeepCopy();
                BitString prevTempIv = null;
                SymmetricCipherResult decryptionResult = null; 

                var lastPlainTexts = new List<BitString>();
                BitString output = null;
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    Debug.WriteLine($"Encrypting {i + 1}.{j + 1} out of {NumberOfCases}.{NUMBER_OF_ITERATIONS}");
                    prevTempIv = tempIv;
                    decryptionResult = _algo.BlockDecrypt(responses[i].Keys, tempCipherText, tempIv);
                    //the result is OUTPUT xored with TEXT. 
                    //Output is needed for the next step, so let's recompute it
                    output = decryptionResult.Result.XOR(tempCipherText);

                    tempCipherText = tempIv.GetDeepCopy();
                    tempIv = output.GetDeepCopy();
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, decryptionResult.Result.GetDeepCopy());
                    }

                }
                responses.Last().PlainText = decryptionResult.Result.GetDeepCopy();

                responses.Add(new AlgoArrayResponse()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses.Last().Keys.GetDeepCopy()), lastPlainTexts).ToOddParityBitString(),
                    CipherText = responses.Last().CipherText.GetDeepCopy().XOR(prevTempIv),
                    IV = output.GetDeepCopy()
                });

            }
            responses.RemoveAt(responses.Count() - 1);
            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
