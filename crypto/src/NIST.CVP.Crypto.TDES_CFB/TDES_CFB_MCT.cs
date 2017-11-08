//using NIST.CVP.Crypto.Core;
//using NIST.CVP.Crypto.TDES;
//using NIST.CVP.Math;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;

//namespace NIST.CVP.Crypto.TDES_CFB
//{
//    public class TDES_CFB_MCT : ITDES_CFB_MCT
//    {
//        private readonly IMonteCarloKeyMaker _keyMaker;
//        private readonly IModeOfOperation _modeOfOperation;

//        private const int NUMBER_OF_CASES = 400;
//        //private const int NUMBER_OF_CASES = 5;
//        private const int NUMBER_OF_ITERATIONS = 10000;
//        private readonly int Shift;


//        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }

//        public TDES_CFB_MCT(IMonteCarloKeyMaker keyMaker, IModeOfOperation modeOfOperation)
//        {
//            _keyMaker = keyMaker;
//            _modeOfOperation = modeOfOperation;
//            Shift = ((CFBMode) modeOfOperation).Shift; //TODO casting is bad, needs to be refactored. Maybe add MCT to the mode itself?

//        }

//        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv)
//        {
//            var responses = new List<AlgoArrayResponse>{
//                new AlgoArrayResponse {
//                    IV = iv,
//                    Keys = keyBits,
//                    PlainText = data
//                }
//            };
//            int numberOfOutputsToSave = 192 / Shift;
//            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;
//            for (var i = 0; i < NumberOfCases; i++)
//            {
//                Debug.WriteLineIf((i+1) % 10 == 0, $"Running MCT Encryption round {i+1} out of {NumberOfCases}");
//                var lastResponse = responses.Last(); 
//                var tempText = lastResponse.PlainText.GetDeepCopy();
//                var tempIv = lastResponse.IV.GetDeepCopy();
//                BitString prevTempIv = null;
//                var lastCipherTexts = new List<BitString>();
//                BitString output = null;
//                var keysForThisRound = responses[i].Keys;
//                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
//                {
//                    prevTempIv = tempIv;
//                    output = _modeOfOperation.BlockEncrypt(keysForThisRound, tempIv, tempText).CipherText;
//                    tempText = tempIv.MSBSubstring(0, Shift);
//                    tempIv = tempIv.Substring(0, 64 - Shift).ConcatenateBits(output);
//                    if (j >= indexAtWhichToStartSaving)
//                    {
//                        lastCipherTexts.Insert(0, output.GetDeepCopy());  
//                    }
//                }
//                lastResponse.CipherText = output;
//                responses.Add(new AlgoArrayResponse()
//                {
//                    Keys = _keyMaker.MixKeys(new TDESKeys(lastResponse.Keys.GetDeepCopy()), lastCipherTexts).ToOddParityBitString(),
//                    PlainText = prevTempIv.GetDeepCopy().MSBSubstring(0, Shift),
//                    IV = tempIv.GetDeepCopy()
//                });

//            }
//            responses.RemoveAt(responses.Count() - 1);
//            return new MCTResult<AlgoArrayResponse>(responses);
//        }

//        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv)
//        {
//            var responses = new List<AlgoArrayResponse>{
//                new AlgoArrayResponse {
//                    IV = iv,
//                    Keys = keyBits,
//                    CipherText = data
//                }
//            };
//            int numberOfOutputsToSave = 192 / Shift;
//            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;
//            for (var i = 0; i < NumberOfCases; i++)
//            {
//                Debug.WriteLineIf((i + 1) % 10 == 0, $"Running MCT Decryption round {i+1} out of {NumberOfCases}");
//                var lastResponse = responses.Last();
//                var tempText = lastResponse.CipherText.GetDeepCopy();
//                var tempIv = lastResponse.IV.GetDeepCopy();
//                var lastPlainTexts = new List<BitString>();
//                BitString output = null;
//                var keysForThisRound = responses[i].Keys;
//                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
//                {
//                    output = _modeOfOperation.BlockDecrypt(keysForThisRound, tempIv, tempText).PlainText;
//                    tempIv = tempIv.Substring(0, 64 - Shift).ConcatenateBits(tempText);
//                    tempText = output.Substring(0, Shift).XOR(tempText);
//                    if (j >= indexAtWhichToStartSaving)
//                    {
//                        lastPlainTexts.Insert(0, output.GetDeepCopy());
//                    }
//                }
//                lastResponse.PlainText = output;
//                responses.Add(new AlgoArrayResponse()
//                {
//                    Keys = _keyMaker.MixKeys(new TDESKeys(lastResponse.Keys.GetDeepCopy()), lastPlainTexts).ToOddParityBitString(),
//                    CipherText = tempText,
//                    IV = tempIv.GetDeepCopy()
//                });
//            }
//            responses.RemoveAt(responses.Count() - 1);
//            return new MCTResult<AlgoArrayResponse>(responses);
//        }
//    }
//}
