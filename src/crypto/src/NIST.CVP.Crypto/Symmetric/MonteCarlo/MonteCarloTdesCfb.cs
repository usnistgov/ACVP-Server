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
    public class MonteCarloTdesCfb : IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;


        protected virtual int NumberOfCases => NUMBER_OF_CASES;
        public int Shift { get; set; }

        public MonteCarloTdesCfb(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker, int shiftSize, BlockCipherModesOfOperation mode)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                mode
            );
            _keyMaker = keyMaker;
            Shift = shiftSize;
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
        
        private Common.Symmetric.MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = param.Iv,
                    Keys = param.Key,
                    PlainText = param.Payload
                }
            };
            int numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;
            for (var i = 0; i < NumberOfCases; i++)
            {
                Debug.WriteLineIf((i + 1) % 10 == 0, $"Running MCT Encryption round {i + 1} out of {NumberOfCases}");
                var lastResponse = responses.Last();
                var tempText = lastResponse.PlainText.GetDeepCopy();
                var tempIv = lastResponse.IV.GetDeepCopy();
                BitString prevTempIv = null;
                var lastCipherTexts = new List<BitString>();
                BitString output = null;
                var keysForThisRound = responses[i].Keys;
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    prevTempIv = tempIv.GetDeepCopy();
                    output = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        tempIv,
                        keysForThisRound,
                        tempText
                    )).Result;
                    tempText = prevTempIv.MSBSubstring(0, Shift);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, output.GetDeepCopy());
                    }
                }
                lastResponse.CipherText = output;
                responses.Add(new AlgoArrayResponse()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(lastResponse.Keys.GetDeepCopy()), lastCipherTexts).ToOddParityBitString(),
                    PlainText = prevTempIv.GetDeepCopy().MSBSubstring(0, Shift),
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
                    IV = param.Iv,
                    Keys = param.Key,
                    CipherText = param.Payload
                }
            };
            int numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;
            for (var i = 0; i < NumberOfCases; i++)
            {
                Debug.WriteLineIf((i + 1) % 10 == 0, $"Running MCT Decryption round {i + 1} out of {NumberOfCases}");
                var lastResponse = responses.Last();
                var tempText = lastResponse.CipherText.GetDeepCopy();
                var tempIv = lastResponse.IV.GetDeepCopy();
                var lastPlainTexts = new List<BitString>();
                BitString output = null;
                var keysForThisRound = responses[i].Keys;
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    output = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        tempIv,
                        keysForThisRound,
                        tempText
                    )).Result;

                    tempText = output.MSBSubstring(0, Shift).XOR(tempText);
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, output.GetDeepCopy());
                    }
                }
                lastResponse.PlainText = output;
                responses.Add(new AlgoArrayResponse()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(lastResponse.Keys.GetDeepCopy()), lastPlainTexts).ToOddParityBitString(),
                    CipherText = tempText,
                    IV = tempIv.GetDeepCopy()
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
