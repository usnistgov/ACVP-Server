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
    public class MonteCarloTdesOfbi : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int PARTITIONS = 3;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public MonteCarloTdesOfbi(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
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
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);

            var responses = new List<AlgoArrayResponse>{
                new AlgoArrayResponse {
                    IV = ivs[0],
                    Keys = param.Key.GetDeepCopy(),
                    PlainText = param.Payload.GetDeepCopy()
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
                    encryptionInput = j < PARTITIONS ? ivs[j] : encryptionOutputs[j - PARTITIONS];
                    var encryptionOutput = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt, key, encryptionInput
                    )).Result;

                    encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                    cipherText = param.Payload.XOR(encryptionOutput);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, cipherText.GetDeepCopy());
                    }
                    param.Payload = encryptionInput.GetDeepCopy();
                }

                responses.Last().CipherText = cipherText.GetDeepCopy();

                ivs = TdesPartitionHelpers.SetupIvs(encryptionOutputs[9995].XOR(cipherText));

                responses.Add(new AlgoArrayResponse
                {
                    IV = ivs[0],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList()).ToOddParityBitString(),
                    PlainText = responses.Last().PlainText.XOR(encryptionInput)
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            var ivs = TdesPartitionHelpers.SetupIvs(param.Iv);

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
                var encryptionOutputs = new List<BitString>();
                var plainText = new BitString(0);
                var encryptionInput = new BitString(0);
                var key = responses.Last().Keys.GetDeepCopy();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    encryptionInput = j < PARTITIONS ? ivs[j] : encryptionOutputs[j - PARTITIONS];
                    var encryptionOutput = _algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt, key, encryptionInput
                    )).Result;

                    encryptionOutputs.Add(encryptionOutput.GetDeepCopy());
                    plainText = param.Payload.XOR(encryptionOutput);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, plainText.GetDeepCopy());
                    }
                    param.Payload = encryptionInput.GetDeepCopy();
                }

                responses.Last().PlainText = plainText.GetDeepCopy();

                ivs = TdesPartitionHelpers.SetupIvs(encryptionOutputs[9995].XOR(plainText));

                responses.Add(new AlgoArrayResponse
                {
                    IV = ivs[0],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList()).ToOddParityBitString(),
                    CipherText = responses.Last().CipherText.XOR(encryptionInput)
                });
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponse>(responses);
        }
    }
}
