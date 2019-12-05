using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloAesEcb : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerAes _keyMaker;

        public MonteCarloAesEcb(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory,
            IMonteCarloKeyMakerAes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                BlockCipherModesOfOperation.Ecb
            );
            _keyMaker = keyMaker;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
        Key[0] = Key
        PT[0] = PT
        For i = 0 to 99
            Output Key[i]
            Output PT[0]
            For j = 0 to 999
                CT[j] = AES(Key[i], PT[j])
                PT[j + 1] = CT[j]
            Output CT[j]
            If(keylen = 128)
                Key[i + 1] = Key[i] xor CT[j]
            If(keylen = 192)
                Key[i + 1] = Key[i] xor(last 64 - bits of CT[j - 1] || CT[j])
            If(keylen = 256)
                Key[i + 1] = Key[i] xor(CT[j - 1] || CT[j])
            PT[0] = CT[j]
        */
        #endregion MonteCarloAlgorithm Pseudocode

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
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;
            try
            {
                for (i = 0; i < 100; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        Key = param.Key,
                        PlainText = param.Payload
                    };

                    BitString jCipherText = null;
                    BitString previousCipherText = null;
                    BitString copyPreviousCipherText = null;
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        jCipherText = jResult.Result;

                        param.Payload = jCipherText;
                        copyPreviousCipherText = previousCipherText;
                        previousCipherText = jCipherText;
                    }

                    iIterationResponse.CipherText = jCipherText;
                    responses.Add(iIterationResponse);

                    param.Key = _keyMaker.MixKeys(param.Key, previousCipherText, copyPreviousCipherText);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;
            try
            {
                for (i = 0; i < 100; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        Key = param.Key,
                        CipherText = param.Payload
                    };

                    BitString jPlainText = null;
                    BitString previousPlainText = null;
                    BitString copyPreviousPlainText = null;
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        jPlainText = jResult.Result;
                        
                        param.Payload = jPlainText;
                        copyPreviousPlainText = previousPlainText;
                        previousPlainText = jPlainText;
                    }

                    iIterationResponse.PlainText = jPlainText;
                    responses.Add(iIterationResponse);

                    param.Key = _keyMaker.MixKeys(param.Key, previousPlainText, copyPreviousPlainText);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
