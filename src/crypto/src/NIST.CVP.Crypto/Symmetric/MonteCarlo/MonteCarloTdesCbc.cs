using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NLog;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Crypto.TDES_CBC
{
    public class MonteCarloTdesCbc : IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;
        
        public MonteCarloTdesCbc(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                BlockCipherModesOfOperation.Cbc
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

        private Common.Symmetric.MCTResult<AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            List<BitString> lastCipherTexts = new List<BitString>();
            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                {
                    IV = param.Iv,
                    Keys = param.Key,
                    PlainText = param.Payload
                };

                BitString jCipherText = null;
                BitString previousCipherText = null;
                var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                param.Iv = new BitString(ivCopiedBytes);

                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    var jResult = _algo.ProcessPayload(param);
                    jCipherText = jResult.Result;
                    SaveOutputForKeyMixing(jResult.Result.GetDeepCopy(), lastCipherTexts);

                    if (innerLoop == 0)
                    {
                        previousCipherText = iIterationResponse.IV;
                    }

                    param.Payload = previousCipherText;
                    previousCipherText = jCipherText;
                }

                // Inner loop complete, save response
                iIterationResponse.CipherText = jCipherText;
                responses.Add(iIterationResponse);

                // Setup next loop values
                param.Key =
                    _keyMaker.MixKeys(new TDESKeys(iIterationResponse.Keys.GetDeepCopy()), lastCipherTexts)
                        .ToOddParityBitString();
            }

            return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);
        }

        private Common.Symmetric.MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            List<BitString> lastPlainTexts = new List<BitString>();
            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                {
                    IV = param.Iv,
                    Keys = param.Key,
                    CipherText = param.Payload
                };

                BitString jPlainText = null;
                var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                param.Iv = new BitString(ivCopiedBytes);

                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    var jResult = _algo.ProcessPayload(param);
                    jPlainText = jResult.Result;
                    SaveOutputForKeyMixing(jResult.Result.GetDeepCopy(), lastPlainTexts);

                    param.Payload = jPlainText;
                }

                // Inner loop complete, save response
                iIterationResponse.PlainText = jPlainText;
                responses.Add(iIterationResponse);

                // Setup next loop values
                param.Key =
                    _keyMaker.MixKeys(new TDESKeys(iIterationResponse.Keys.GetDeepCopy()), lastPlainTexts)
                        .ToOddParityBitString();

            }

            return new Common.Symmetric.MCTResult<AlgoArrayResponse>(responses);
        }

        private void SaveOutputForKeyMixing(BitString output, List<BitString> lastAlgoOutputs)
        {
            lastAlgoOutputs.Insert(0, output);
            if (lastAlgoOutputs.Count > NUMBER_OF_OUTPUTS_TO_SAVE)
            {
                lastAlgoOutputs.RemoveRange(NUMBER_OF_OUTPUTS_TO_SAVE, lastAlgoOutputs.Count - NUMBER_OF_OUTPUTS_TO_SAVE);
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
