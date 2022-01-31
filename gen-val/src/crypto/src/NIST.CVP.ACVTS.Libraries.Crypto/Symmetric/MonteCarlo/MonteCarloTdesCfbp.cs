using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using AlgoArrayResponse = NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloTdesCfbp : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;
        public int Shift { get; set; }

        public MonteCarloTdesCfbp(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker, BlockCipherModesOfOperation mode)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                mode
            );
            _keyMaker = keyMaker;
            switch (mode)
            {
                case BlockCipherModesOfOperation.CfbpBit:
                    Shift = 1;
                    break;
                case BlockCipherModesOfOperation.CfbpByte:
                    Shift = 8;
                    break;
                case BlockCipherModesOfOperation.CfbpBlock:
                    Shift = 64;
                    break;
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }


        public MCTResult<AlgoArrayResponse> ProcessMonteCarloTest(IModeBlockCipherParameters param)
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

        public MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = ivs[0],
                    Keys = param.Key,
                    PlainText = param.Payload
                }
            };
            var numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;

            for (var i = 0; i < NumberOfCases; i++)
            {
                ivs = TdesPartitionHelpers.SetupIvs(responses[i].IV.GetDeepCopy());
                var tempText = responses[i].PlainText.GetDeepCopy();
                BitString prevTempIv = null;
                var tempIv = responses[i].IV.GetDeepCopy();
                var keysForThisRound = responses[i].Keys;
                BitString output = null;
                var holdouts = new BitString[3];
                var lastCipherTexts = new List<BitString>();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    switch (j)
                    {
                        case 0:
                            tempIv = ivs[0].GetDeepCopy();
                            break;
                        case 1:
                            tempIv = ivs[1].GetDeepCopy();
                            break;
                        case 2:
                            tempIv = ivs[2].GetDeepCopy();
                            break;
                        default:
                            tempIv = prevTempIv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(holdouts[2]);
                            break;
                    }
                    prevTempIv = tempIv.GetDeepCopy();

                    output = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt, tempIv, keysForThisRound, tempText)
                    ).Result;

                    holdouts[2] = holdouts[1];
                    holdouts[1] = holdouts[0];
                    holdouts[0] = output;

                    tempText = prevTempIv.MSBSubstring(0, Shift);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, output.GetDeepCopy());
                    }

                }
                responses[i].CipherText = output;

                var newIv = prevTempIv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(output);
                var newIvs = TdesPartitionHelpers.SetupIvs(newIv);

                responses.Add(new AlgoArrayResponse()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList())
                        .ToOddParityBitString(),
                    PlainText = prevTempIv.GetDeepCopy().MSBSubstring(0, Shift),
                    IV = newIvs[0]
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }

        public MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);
            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = ivs[0],
                    Keys = param.Key,
                    CipherText = param.Payload
                }
            };

            var holdouts = new BitString[3];
            var numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;

            for (var i = 0; i < NumberOfCases; i++)
            {
                ivs = TdesPartitionHelpers.SetupIvs(responses[i].IV.GetDeepCopy());
                var tempText = responses[i].CipherText.GetDeepCopy();
                BitString prevTempIv = null;
                var tempIv = responses[i].IV.GetDeepCopy();
                var keysForThisRound = responses[i].Keys;
                BitString output = null;
                var lastPlainTexts = new List<BitString>();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    switch (j)
                    {
                        case 0:
                            tempIv = ivs[0].GetDeepCopy();
                            break;
                        case 1:
                            tempIv = ivs[1].GetDeepCopy();
                            break;
                        case 2:
                            tempIv = ivs[2].GetDeepCopy();
                            break;
                        default:
                            tempIv = prevTempIv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(holdouts[2]);
                            break;
                    }
                    prevTempIv = tempIv.GetDeepCopy();

                    output = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt, tempIv, keysForThisRound, tempText)
                    ).Result;

                    holdouts[2] = holdouts[1];
                    holdouts[1] = holdouts[0];
                    holdouts[0] = tempText;

                    tempText = tempText.XOR(output);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, output.GetDeepCopy());
                    }

                }
                responses[i].PlainText = output;

                var newIv = prevTempIv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(tempText.XOR(output));
                var newIvs = TdesPartitionHelpers.SetupIvs(newIv);
                responses.Add(new AlgoArrayResponse()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList())
                        .ToOddParityBitString(),
                    CipherText = tempText,
                    IV = newIvs[0]
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
