using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_CFB8
{
    public class AES_CFB8_MCT : IAES_CFB8_MCT
    {
        private readonly IAES_CFB8 _algo;
        private const int _OUTPUT_ITERATIONS = 100;
        private const int _INNER_ITERATIONS_PER_OUTPUT = 1000;

        public AES_CFB8_MCT(IAES_CFB8 iAES_CFB8)
        {
            _algo = iAES_CFB8;
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
                    PT[j+1] = ByteJ(IV[i])
                Else
                    CT[j] = AES(Key[i], PT[j])
                    If ( j<16 )
                        PT[j+1] = ByteJ(IV[i])
                    Else
                        PT[j+1] = CT[j-16]
            Output CT[j]
            If ( keylen = 128 )
                Key[i+1] = Key[i] xor (CT[j-15] || CT[j-14] || … || CT[j])
            If ( keylen = 192 )
                Key[i+1] = Key[i] xor (CT[j-23] || CT[j-22] || … || CT[j])
            If ( keylen = 256 )
                Key[i+1] = Key[i] xor (CT[j-31] || CT[j-30] || … || CT[j])
            IV[i+1] = (CT[j-15] || CT[j-14] || … || CT[j])
            PT[0] = CT[j-16]
        */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;

            try
            {
                for (i = 0; i < _OUTPUT_ITERATIONS; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        IV = iv,
                        Key = key,
                        PlainText = plainText
                    };
                    responses.Add(iIterationResponse);

                    List<BitString> previousCipherTexts = new List<BitString>();
                    iv = iv.GetDeepCopy();
                    plainText = plainText.GetDeepCopy();
                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.BlockEncrypt(iv, key, plainText);
                        var jCipherText = jResult.Result.GetDeepCopy();
                        previousCipherTexts.Add(jCipherText);
                        iIterationResponse.CipherText = jCipherText;

                        if (j < 16)
                        {
                            plainText = new BitString(new byte[] {iIterationResponse.IV[j]});
                        }
                        else
                        {
                            plainText = previousCipherTexts[j - 16].GetDeepCopy();
                        }

                        SetupNextOuterLoopValues(ref iv, ref key, ref plainText, j, previousCipherTexts);
                    }
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

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;

            try
            {
                for (i = 0; i < _OUTPUT_ITERATIONS; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        IV = iv,
                        Key = key,
                        CipherText = cipherText
                    };
                    responses.Add(iIterationResponse);

                    List<BitString> previousPlainTexts = new List<BitString>();
                    iv = iv.GetDeepCopy();
                    cipherText = cipherText.GetDeepCopy();
                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.BlockDecrypt(iv, key, cipherText);
                        var jPlainText = jResult.Result.GetDeepCopy();
                        previousPlainTexts.Add(jPlainText);
                        iIterationResponse.PlainText = jPlainText;

                        if (j < 16)
                        {
                            cipherText = new BitString(new byte[] { iIterationResponse.IV[j] });
                        }
                        else
                        {
                            cipherText = previousPlainTexts[j - 16].GetDeepCopy();
                        }

                        SetupNextOuterLoopValues(ref iv, ref key, ref cipherText, j, previousPlainTexts);
                    }
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

        private void SetupNextOuterLoopValues(ref BitString iv, ref BitString key, ref BitString input, int j, List<BitString> previousOutputs)
        {
            if (j == _INNER_ITERATIONS_PER_OUTPUT - 1)
            {
                var index = j - (key.BitLength / 8) + 1;
                var keyConcatenation = ConcatenateOutputsStartingAtIndex(previousOutputs, index);
                key = key.XOR(keyConcatenation).GetDeepCopy();

                iv = ConcatenateOutputsStartingAtIndex(previousOutputs, j - (128 / 8) + 1).GetDeepCopy();
                input = previousOutputs[j - 16].GetDeepCopy();
            }
        }

        private BitString ConcatenateOutputsStartingAtIndex(List<BitString> previousOutputs, int startingIndex)
        {
            BitString bs = previousOutputs[startingIndex];

            for (int iterator = startingIndex + 1; iterator < previousOutputs.Count; iterator++)
            {
                bs = bs.ConcatenateBits(previousOutputs[iterator]);
            }

            return bs;
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
