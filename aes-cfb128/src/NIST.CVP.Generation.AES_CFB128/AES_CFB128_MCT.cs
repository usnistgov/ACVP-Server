using System;
using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class AES_CFB128_MCT : IAES_CFB128_MCT
    {
        private readonly IAES_CFB128 _algo;
        private const int _OUTPUT_ITERATIONS = 100;
        private const int _INNER_ITERATIONS_PER_OUTPUT = 1000;

        public AES_CFB128_MCT(IAES_CFB128 iAES_CFB8)
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

                    BitString previousCipherText = null;
                    BitString currentCipherText = null;
                    iv = iv.GetDeepCopy();
                    plainText = plainText.GetDeepCopy();

                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.BlockEncrypt(iv, key, plainText);
                        currentCipherText = jResult.CipherText.GetDeepCopy();
                        
                        iIterationResponse.CipherText = currentCipherText;

                        if (j == 0)
                        {
                            plainText = iIterationResponse.IV;
                        }
                        else
                        {
                            plainText = previousCipherText.GetDeepCopy();
                        }

                        // The previous cipherText is recorded for use in the next iteration of the loop.  
                        // If the last iteration, we do not want to overwrite the current value.
                        if (j != _INNER_ITERATIONS_PER_OUTPUT - 1)
                        {
                            previousCipherText = currentCipherText.GetDeepCopy();
                        }
                    }

                    SetupOuterLoopInputs(ref iv, ref key, ref plainText, previousCipherText, currentCipherText);
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

                    BitString previousPlainText = null;
                    BitString currentPlainText = null;
                    iv = iv.GetDeepCopy();
                    cipherText = cipherText.GetDeepCopy();

                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.BlockDecrypt(iv, key, cipherText);
                        currentPlainText = jResult.PlainText.GetDeepCopy();

                        iIterationResponse.PlainText = currentPlainText;

                        if (j == 0)
                        {
                            cipherText = iIterationResponse.IV;
                        }
                        else
                        {
                            cipherText = previousPlainText.GetDeepCopy();
                        }

                        // The previous cipherText is recorded for use in the next iteration of the loop.  
                        // If the last iteration, we do not want to overwrite the current value.
                        if (j != _INNER_ITERATIONS_PER_OUTPUT - 1)
                        {
                            previousPlainText = currentPlainText.GetDeepCopy();
                        }
                    }

                    SetupOuterLoopInputs(ref iv, ref key, ref cipherText, previousPlainText, currentPlainText);
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

        private static void SetupOuterLoopInputs(ref BitString iv, ref BitString key, ref BitString input, BitString previousOutput, BitString currentOutput)
        {
            if (key.BitLength == 128)
            {
                key = key.XOR(currentOutput);
            }
            if (key.BitLength == 192)
            {
                key = key.XOR(
                    previousOutput.GetLeastSignificantBits(64).ConcatenateBits(currentOutput)
                );
            }
            if (key.BitLength == 256)
            {
                key = key.XOR(
                    previousOutput.ConcatenateBits(currentOutput)
                );
            }

            iv = currentOutput.GetDeepCopy();
            input = previousOutput.GetDeepCopy();
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
