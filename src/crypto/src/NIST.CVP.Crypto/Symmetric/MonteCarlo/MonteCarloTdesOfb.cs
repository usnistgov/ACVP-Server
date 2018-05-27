using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloTdesOfb : IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public MonteCarloTdesOfb(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                BlockCipherModesOfOperation.Ofb
            );
            _keyMaker = keyMaker;
        }

        public Common.Symmetric.MCTResult<AlgoArrayResponse> ProcessMonteCarloTest(IModeBlockCipherParameters param)
        {
            switch (param.Direction)
            {
                case BlockCipherDirections.Encrypt:
                    return Encrypt(param);
                case BlockCipherDirections.Decrypt:
                    return Decrypt(param);
                default:
                    throw new ArgumentException(nameof(param.Direction));
            }
        }

        //private Common.Symmetric.MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        //{
        //    var responses = new List<AlgoArrayResponse>{
        //        new AlgoArrayResponse {
        //            IV = param.Iv.GetDeepCopy(),
        //            Keys = param.Key.GetDeepCopy(),
        //            PlainText = param.Payload.GetDeepCopy()
        //        }
        //    };

        //    var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
        //    for (var i = 0; i < NumberOfCases; i++)
        //    {

        //        var tempText = responses.Last().PlainText.GetDeepCopy();
        //        var tempIv = responses.Last().IV.GetDeepCopy();
        //        BitString prevTempIv = null;
        //        SymmetricCipherResult encryptionResult = null;

        //        var lastCipherTexts = new List<BitString>();
        //        BitString output = null;
        //        for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
        //        {
        //            prevTempIv = tempIv;
        //            encryptionResult = _algo.ProcessPayload(new ModeBlockCipherParameters(
        //                BlockCipherDirections.Encrypt,
        //                tempIv, 
        //                responses[i].Keys, 
        //                tempText
        //            ));

        //            //the result is OUTPUT xored with TEXT. 
        //            //Output is needed for the next step, so let's recompute it
        //            output = encryptionResult.Result.XOR(tempText);

        //            tempText = tempIv.GetDeepCopy();
        //            tempIv = output.GetDeepCopy();
        //            if (j >= indexAtWhichToStartSaving)
        //            {
        //                lastCipherTexts.Insert(0, encryptionResult.Result.GetDeepCopy());
        //            }
        //        }
        //        responses.Last().CipherText = encryptionResult.Result.GetDeepCopy();

        //        responses.Add(new AlgoArrayResponse()
        //            {
        //                Keys = _keyMaker.MixKeys(new TDESKeys(responses.Last().Keys.GetDeepCopy()), lastCipherTexts).ToOddParityBitString(),
        //                PlainText = responses.Last().PlainText.GetDeepCopy().XOR(prevTempIv),
        //                IV = output.GetDeepCopy()
        //        });

        //    }
        //    responses.RemoveAt(responses.Count() - 1);
        //    return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);

        //}

        //private Common.Symmetric.MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        //{
        //    var responses = new List<AlgoArrayResponse>{
        //        new AlgoArrayResponse {
        //            IV = param.Iv.GetDeepCopy(),
        //            Keys = param.Key.GetDeepCopy(),
        //            CipherText = param.Payload.GetDeepCopy()
        //        }
        //    };

        //    var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
        //    for (var i = 0; i < NumberOfCases; i++)
        //    {

        //        var tempCipherText = responses.Last().CipherText.GetDeepCopy();
        //        var tempIv = responses.Last().IV.GetDeepCopy();
        //        BitString prevTempIv = null;
        //        SymmetricCipherResult decryptionResult = null; 

        //        var lastPlainTexts = new List<BitString>();
        //        BitString output = null;
        //        for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
        //        {
        //            Debug.WriteLine($"Encrypting {i + 1}.{j + 1} out of {NumberOfCases}.{NUMBER_OF_ITERATIONS}");
        //            prevTempIv = tempIv;
        //            decryptionResult = _algo.ProcessPayload(new ModeBlockCipherParameters(
        //                BlockCipherDirections.Decrypt,
        //                tempIv,
        //                responses[i].Keys,
        //                tempCipherText
        //            ));

        //            //the result is OUTPUT xored with TEXT. 
        //            //Output is needed for the next step, so let's recompute it
        //            output = decryptionResult.Result.XOR(tempCipherText);

        //            tempCipherText = tempIv.GetDeepCopy();
        //            tempIv = output.GetDeepCopy();
        //            if (j >= indexAtWhichToStartSaving)
        //            {
        //                lastPlainTexts.Insert(0, decryptionResult.Result.GetDeepCopy());
        //            }

        //        }
        //        responses.Last().PlainText = decryptionResult.Result.GetDeepCopy();

        //        responses.Add(new AlgoArrayResponse()
        //        {
        //            Keys = _keyMaker.MixKeys(new TDESKeys(responses.Last().Keys.GetDeepCopy()), lastPlainTexts).ToOddParityBitString(),
        //            CipherText = responses.Last().CipherText.GetDeepCopy().XOR(prevTempIv),
        //            IV = output.GetDeepCopy()
        //        });

        //    }
        //    responses.RemoveAt(responses.Count() - 1);
        //    return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);
        //}

        private Common.Symmetric.MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = param.Iv.GetDeepCopy(),
                    Keys = param.Key.GetDeepCopy(),
                    PlainText = param.Payload.GetDeepCopy()
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
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    prevTempIv = tempIv.GetDeepCopy();
                    encryptionResult = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        tempIv,
                        responses[i].Keys,
                        tempText
                    ));

                    tempText = prevTempIv.GetDeepCopy();
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
                    IV = tempIv.GetDeepCopy()
                });

            }
            responses.RemoveAt(responses.Count() - 1);
            return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);

        }

        private Common.Symmetric.MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = param.Iv.GetDeepCopy(),
                    Keys = param.Key.GetDeepCopy(),
                    CipherText = param.Payload.GetDeepCopy()
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
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    Debug.WriteLine($"Encrypting {i + 1}.{j + 1} out of {NumberOfCases}.{NUMBER_OF_ITERATIONS}");
                    prevTempIv = tempIv.GetDeepCopy();
                    decryptionResult = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        tempIv,
                        responses[i].Keys,
                        tempCipherText
                    ));

                    tempCipherText = prevTempIv.GetDeepCopy();
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
                    IV = tempIv.GetDeepCopy()
                });

            }
            responses.RemoveAt(responses.Count() - 1);
            return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
