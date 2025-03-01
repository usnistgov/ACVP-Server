﻿using System;
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
    public class MonteCarloTdesCbci : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int PARTITIONS = 3;
        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public MonteCarloTdesCbci(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                BlockCipherModesOfOperation.Ecb
            );
            _keyMaker = keyMaker;
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

        private MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            if (param.Payload.BitLength != 192)
            {
                throw new ArgumentException("Data must be 192 bits");
            }
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);
            var pts = TdesPartitionHelpers.TriPartitionBitString(param.Payload);

            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = ivs[0],
                    Keys = param.Key,
                    PlainText = param.Payload
                }
            };
            var lastCipherTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    for (var k = 0; k < PARTITIONS; k++)
                    {
                        var result = _algo.ProcessPayload(new ModeBlockCipherParameters(
                            BlockCipherDirections.Encrypt,
                            responses[i].Keys,
                            ivs[k].XOR(pts[k])
                        )).Result;
                        pts[k] = ivs[k].GetDeepCopy();
                        ivs[k] = result.GetDeepCopy();
                    }
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, ivs[NUMBER_OF_ITERATIONS - 1 - j].GetDeepCopy());
                    }
                }

                responses.Last().CipherText = ivs[0].ConcatenateBits(ivs[1]).ConcatenateBits(ivs[2]);

                responses.Add(new AlgoArrayResponse
                {
                    IV = ivs[0],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList()).ToOddParityBitString(),
                    PlainText = pts[0].ConcatenateBits(pts[1].ConcatenateBits(pts[2]))
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            if (param.Payload.BitLength != 192)
            {
                throw new ArgumentException("Data must be 192 bits");
            }
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);
            var cts = TdesPartitionHelpers.TriPartitionBitString(param.Payload);

            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = ivs[0],
                    Keys = param.Key,
                    CipherText = param.Payload
                }
            };
            var lastPlainTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var result = _algo.ProcessPayload(new ModeBlockCipherParameters(
                            BlockCipherDirections.Decrypt,
                            responses[i].Keys,
                            cts[k]
                        )).Result.XOR(ivs[k]);
                        ivs[k] = cts[k].GetDeepCopy();
                        cts[k] = result.GetDeepCopy();
                    }
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, cts[NUMBER_OF_ITERATIONS - 1 - j].GetDeepCopy());
                    }
                }

                responses.Last().PlainText = cts[0].ConcatenateBits(cts[1]).ConcatenateBits(cts[2]);

                responses.Add(new AlgoArrayResponse
                {
                    IV = ivs[0],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList()).ToOddParityBitString(),
                    CipherText = cts[0].ConcatenateBits(cts[1].ConcatenateBits(cts[2]))
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
