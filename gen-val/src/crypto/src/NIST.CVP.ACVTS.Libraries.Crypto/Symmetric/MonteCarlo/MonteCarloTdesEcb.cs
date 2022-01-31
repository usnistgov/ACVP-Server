using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;
using AlgoArrayResponse = NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AlgoArrayResponse;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloTdesEcb : IMonteCarloTester<Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse>, Common.Symmetric.TDES.AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerTdes _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public MonteCarloTdesEcb(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                BlockCipherModesOfOperation.Ecb
            );
            _keyMaker = keyMaker;
        }

        public Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse> ProcessMonteCarloTest(IModeBlockCipherParameters param)
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

        private Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse> Encrypt(IModeBlockCipherParameters param)
        {
            var responses = new List<Common.Symmetric.TDES.AlgoArrayResponse>();

            var lastCipherTexts = new List<BitString>();
            for (var outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                var iIterationResponse = new Common.Symmetric.TDES.AlgoArrayResponse
                {
                    Keys = param.Key,
                    PlainText = param.Payload
                };

                BitString jCipherText = null;
                for (var innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    var jResult = _algo.ProcessPayload(param);
                    jCipherText = jResult.Result;
                    SaveOutputForKeyMixing(jResult.Result.GetDeepCopy(), lastCipherTexts);

                    param.Payload = jCipherText;
                }

                // Inner loop complete, save response
                iIterationResponse.CipherText = jCipherText;
                responses.Add(iIterationResponse);

                // Setup next loop values
                param.Key =
                    _keyMaker.MixKeys(new TDESKeys(iIterationResponse.Keys.GetDeepCopy()), lastCipherTexts)
                        .ToOddParityBitString();
            }

            return new Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse>(responses);
        }

        private Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            var responses = new List<Common.Symmetric.TDES.AlgoArrayResponse>();

            var lastPlainTexts = new List<BitString>();
            for (var outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                var iIterationResponse = new Common.Symmetric.TDES.AlgoArrayResponse
                {
                    Keys = param.Key,
                    CipherText = param.Payload
                };

                BitString jPlainText = null;
                for (var innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
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

            return new Common.Symmetric.MCTResult<Common.Symmetric.TDES.AlgoArrayResponse>(responses);
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
