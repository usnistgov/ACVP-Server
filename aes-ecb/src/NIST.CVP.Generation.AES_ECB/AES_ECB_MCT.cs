using System;
using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class AES_ECB_MCT : IAES_ECB_MCT
    {
        private readonly IAES_ECB _iAES_ECB;

        public AES_ECB_MCT(IAES_ECB iAES_ECB)
        {
            _iAES_ECB = iAES_ECB;
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

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString plainText)
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
                        Key = key,
                        PlainText = plainText
                    };

                    BitString jCipherText = null;
                    BitString previousCipherText = null;
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _iAES_ECB.BlockEncrypt(key, plainText);
                        jCipherText = jResult.CipherText;
                        previousCipherText = plainText;
                        plainText = jCipherText;
                    }

                    iIterationResponse.CipherText = jCipherText;
                    responses.Add(iIterationResponse);

                    if (key.BitLength == 128)
                    {
                        key = key.XOR(jCipherText);
                    }
                    if (key.BitLength == 192)
                    {
                        var mostSignificant16KeyBitStringXor =
                            key.GetMostSignificantBits(8 * 8).XOR( // XOR 64 most significant key bits w/
                                previousCipherText.Substring(0, 64) // the 64 least significant bits of the previous cipher text
                            );
                        var leastSignificant128KeyBitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(jCipherText);

                        key = mostSignificant16KeyBitStringXor.ConcatenateBits(leastSignificant128KeyBitStringXor);
                    }
                    if (key.BitLength == 256)
                    {
                        var mostSignificantFirst16BitStringXor = key.GetMostSignificantBits(16 * 8).XOR(previousCipherText);
                        var leastSignificant16BitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(jCipherText);
                        key = mostSignificantFirst16BitStringXor.ConcatenateBits(leastSignificant16BitStringXor);
                    }

                    plainText = jCipherText;
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

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString cipherText)
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
                        Key = key,
                        CipherText = cipherText
                    };

                    BitString jPlainText = null;
                    BitString previousPlainText = null;
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _iAES_ECB.BlockDecrypt(key, cipherText);
                        jPlainText = jResult.PlainText;
                        previousPlainText = cipherText;
                        cipherText = jPlainText;
                    }

                    iIterationResponse.PlainText = jPlainText;
                    responses.Add(iIterationResponse);

                    if (key.BitLength == 128)
                    {
                        key = key.XOR(jPlainText);
                    }
                    if (key.BitLength == 192)
                    {
                        var mostSignificant16KeyBitStringXor =
                            key.GetMostSignificantBits(8 * 8).XOR( // XOR 64 most significant key bits w/
                                previousPlainText.Substring(0, 64) // the 64 least significant bits of the previous cipher text
                            );
                        var leastSignificant128KeyBitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(jPlainText);

                        key = mostSignificant16KeyBitStringXor.ConcatenateBits(leastSignificant128KeyBitStringXor);
                    }
                    if (key.BitLength == 256)
                    {
                        var mostSignificantFirst16BitStringXor = key.GetMostSignificantBits(16 * 8).XOR(previousPlainText);
                        var leastSignificant16BitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(jPlainText);
                        key = mostSignificantFirst16BitStringXor.ConcatenateBits(leastSignificant16BitStringXor);
                    }

                    cipherText = jPlainText;
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
