using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloAesCfb : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private const int _OUTPUT_ITERATIONS = 100;
        private const int _INNER_ITERATIONS_PER_OUTPUT = 1000;

        public readonly int Shift;

        public MonteCarloAesCfb(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, int shiftSize, BlockCipherModesOfOperation mode)
        {
            _algo = modeFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                mode
            );
            Shift = shiftSize;
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
			        PT[j+1] = BitJ(IV[i])
		        Else
			        CT[j] = AES(Key[i], PT[j])
			        If ( j<128 )
				        PT[j+1] = BitJ(IV[i])
			        Else
				        PT[j+1] = CT[j-128]
	        Output CT[j]
		        If ( keylen = 128 )
			        Key[i+1] = Key[i] xor (CT[j-127] || CT[j-126] || … || CT[j])
		        If ( keylen = 192 )
			        Key[i+1] = Key[i] xor (CT[j-191] || CT[j-190] || … || CT[j])
		        If ( keylen = 256 ) Key[i+1] = Key[i] xor (CT[j-255] || CT[j-254] || … || CT[j])
	        IV[i+1] = (CT[j-127] || CT[j-126] || … || CT[j])
	        PT[0] = CT[j-128]
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
                for (i = 0; i < _OUTPUT_ITERATIONS; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        IV = param.Iv,
                        Key = param.Key,
                        PlainText = param.Payload
                    };
                    responses.Add(iIterationResponse);

                    List<BitString> previousCipherTexts = new List<BitString>();
                    param.Iv = param.Iv.GetDeepCopy();
                    param.Payload = param.Payload.GetDeepCopy();
                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        var jCipherText = jResult.Result.GetDeepCopy();
                        previousCipherTexts.Add(jCipherText);
                        iIterationResponse.CipherText = jCipherText;

                        if (j < 128)
                        {
                            // Note, Bits are stored in the opposite direction on the BitString in comparison to where the MCT pseudo code expects them
                            param.Payload = iIterationResponse.IV.Substring(iIterationResponse.IV.BitLength - 1 - j, 1).GetDeepCopy();
                        }
                        else
                        {
                            param.Payload = previousCipherTexts[j - 128].GetDeepCopy();
                        }

                        BitString iv = param.Iv.GetDeepCopy();
                        BitString key = param.Key.GetDeepCopy();
                        BitString payload = param.Payload.GetDeepCopy();
                        SetupNextOuterLoopValues(ref iv, ref key, ref payload, j, previousCipherTexts);
                        param.Iv = iv.GetDeepCopy();
                        param.Key = key.GetDeepCopy();
                        param.Payload = payload.GetDeepCopy();
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

        private MCTResult<AlgoArrayResponse> Decrypt(IModeBlockCipherParameters param)
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
                        IV = param.Iv,
                        Key = param.Key,
                        CipherText = param.Payload
                    };
                    responses.Add(iIterationResponse);

                    List<BitString> previousPlainTexts = new List<BitString>();
                    param.Iv = param.Iv.GetDeepCopy();
                    param.Payload = param.Payload.GetDeepCopy();
                    for (j = 0; j < _INNER_ITERATIONS_PER_OUTPUT; j++)
                    {
                        var jResult = _algo.ProcessPayload(param);
                        var jPlainText = jResult.Result.GetDeepCopy();
                        previousPlainTexts.Add(jPlainText);
                        iIterationResponse.PlainText = jPlainText;

                        if (j < 128)
                        {
                            // Note, Bits are stored in the opposite direction on the BitString in comparison to where the MCT pseudo code expects them
                            param.Payload = iIterationResponse.IV.Substring(iIterationResponse.IV.BitLength - 1 - j, 1).GetDeepCopy();
                        }
                        else
                        {
                            param.Payload = previousPlainTexts[j - 128].GetDeepCopy();
                        }

                        BitString iv = param.Iv.GetDeepCopy();
                        BitString key = param.Key.GetDeepCopy();
                        BitString payload = param.Payload.GetDeepCopy();
                        SetupNextOuterLoopValues(ref iv, ref key, ref payload, j, previousPlainTexts);
                        param.Iv = iv.GetDeepCopy();
                        param.Key = key.GetDeepCopy();
                        param.Payload = payload.GetDeepCopy();
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
                var len = j - key.BitLength + 1;
                var keyConcatenation = ConcatenateOutputsStartingAtIndex(previousOutputs, len);
                key = key.XOR(keyConcatenation).GetDeepCopy();

                iv = ConcatenateOutputsStartingAtIndex(previousOutputs, j - 128 + 1).GetDeepCopy();
                input = previousOutputs[j - 128].GetDeepCopy();
            }
        }

        private BitString ConcatenateOutputsStartingAtIndex(List<BitString> previousOutputs, int startingIndex)
        {
            BitString bs = previousOutputs[startingIndex].GetMostSignificantBits(1);

            for (int iterator = startingIndex + 1; iterator < previousOutputs.Count; iterator++)
            {
                bs = bs.ConcatenateBits(previousOutputs[iterator].GetMostSignificantBits(1));
            }

            return bs;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
