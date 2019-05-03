using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloAesCbc : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerAes _keyMaker;

        public MonteCarloAesCbc(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerAes keyMaker)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                BlockCipherModesOfOperation.Cbc
            );
            _keyMaker = keyMaker;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
        Key[0] = Key
        IV[0] = IV
        PT[0] = PT
        For i = 0 to 99
            Output Key[i]
            Output IV[i]
            Output PT[0]
            For j = 0 to 999
                If ( j=0 )
                    CT[j] = AES(Key[i], IV[i], PT[j])
                    PT[j+1] = IV[i]
                Else
                    CT[j] = AES(Key[i], PT[j])
                    PT[j+1] = CT[j-1]
            Output CT[j]
            If ( keylen = 128 )
                Key[i+1] = Key[i] xor CT[j]
            If ( keylen = 192 )
                Key[i+1] = Key[i] xor (last 64-bits of CT[j-1] || CT[j])
            If ( keylen = 256 )
                Key[i+1] = Key[i] xor (CT[j-1] || CT[j])
            IV[i+1] = CT[j]
            PT[0] = CT[j-1]
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
                        IV = param.Iv,
                        Key = param.Key,
                        PlainText = param.Payload
                    };

                    BitString jCipherText = null;
                    BitString previousCipherText = null;
                    BitString copyPreviousCipherText = null;
                    var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                    param.Iv = new BitString(ivCopiedBytes);
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        jCipherText = jResult.Result;

                        if (j == 0)
                        {
                            previousCipherText = iIterationResponse.IV;
                        }

                        param.Payload = previousCipherText;
                        copyPreviousCipherText = previousCipherText;
                        previousCipherText = jCipherText;
                    }

                    iIterationResponse.CipherText = jCipherText;
                    responses.Add(iIterationResponse);

                    param.Key = _keyMaker.MixKeys(param.Key, previousCipherText, copyPreviousCipherText);
                    param.Iv = previousCipherText;
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
                        IV = param.Iv,
                        Key = param.Key,
                        CipherText = param.Payload
                    };

                    BitString jPlainText = null;
                    BitString previousPlainText = null;
                    BitString copyPreviousPlainText = null;
                    var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                    param.Iv = new BitString(ivCopiedBytes);
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        jPlainText = jResult.Result;

                        if (j == 0)
                        {
                            previousPlainText = iIterationResponse.IV;
                        }

                        param.Payload = previousPlainText;
                        copyPreviousPlainText = previousPlainText;
                        previousPlainText = jPlainText;
                    }

                    iIterationResponse.PlainText = jPlainText;
                    responses.Add(iIterationResponse);

                    param.Key = _keyMaker.MixKeys(param.Key, previousPlainText, copyPreviousPlainText);
                    param.Iv = previousPlainText;
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



        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
