using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo
{
    public class MonteCarloAesCfb : IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloKeyMakerAes _keyMaker;
        private readonly int _blockSizeBits;
        private const int _OUTPUT_ITERATIONS = 100;
        private const int _INNER_ITERATIONS_PER_OUTPUT = 1000;

        public readonly int Shift;

        public MonteCarloAesCfb(
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            IMonteCarloKeyMakerAes keyMaker,
            int shiftSize,
            BlockCipherModesOfOperation mode
        )
        {
            var engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            _algo = modeFactory.GetStandardCipher(
                engine,
                mode
            );
            _keyMaker = keyMaker;
            _blockSizeBits = engine.BlockSizeBits;
            Shift = shiftSize;
        }

        #region MonteCarloAlgorithm CFB1 Pseudocode
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
        #endregion MonteCarloAlgorithm CFB1 Pseudocode

        #region MonteCarloAlgorithm CFB8 Pseudocode
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
        #endregion MonteCarloAlgorithm CFB8 Pseudocode

        #region MonteCarloAlgorithm CFB128 Pseudocode
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
        #endregion MonteCarloAlgorithm CFB128 Pseudocode

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

                        param.Payload = GetNextPayload(j, iIterationResponse.IV, previousCipherTexts);

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

                        param.Payload = GetNextPayload(j, iIterationResponse.IV, previousPlainTexts);

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
                switch (Shift)
                {
                    case 1:
                        {
                            var len = j - key.BitLength + 1;
                            var keyConcatenation = ConcatenateOutputsStartingAtIndex(previousOutputs, len);
                            key = key.XOR(keyConcatenation).GetDeepCopy();

                            iv = ConcatenateOutputsStartingAtIndex(previousOutputs, j - (_blockSizeBits / Shift) + 1).GetDeepCopy();
                            input = previousOutputs[j - _blockSizeBits / Shift].GetDeepCopy();
                            break;
                        }

                    case 8:
                        {
                            var index = j - (key.BitLength / 8) + 1;
                            var keyConcatenation = ConcatenateOutputsStartingAtIndex(previousOutputs, index);
                            key = key.XOR(keyConcatenation).GetDeepCopy();

                            iv = ConcatenateOutputsStartingAtIndex(previousOutputs, j - (128 / 8) + 1).GetDeepCopy();
                            input = previousOutputs[j - 16].GetDeepCopy();
                            break;
                        }

                    case 128:
                        {
                            int previousOutputsLastIndex = previousOutputs.Count - 1;
                            var currentOutput = previousOutputs[previousOutputsLastIndex];
                            var previousOutput = previousOutputs[previousOutputsLastIndex - 1];

                            key = _keyMaker.MixKeys(
                                key,
                                currentOutput,
                                previousOutput
                            );

                            iv = currentOutput;
                            input = previousOutput;
                            break;
                        }

                    default:
                        throw new ArgumentException(nameof(Shift));
                }
            }
        }

        private BitString GetNextPayload(int j, BitString currentIv, List<BitString> previousOutputs)
        {
            switch (Shift)
            {
                case 1:
                    if (j < 128)
                    {
                        // Note, Bits are stored in the opposite direction on the BitString in comparison to where the MCT pseudo code expects them
                        return currentIv
                            .Substring(currentIv.BitLength - 1 - j, Shift).GetDeepCopy();
                    }
                    else
                    {
                        return previousOutputs[j - _blockSizeBits / Shift].GetDeepCopy();
                    }
                case 8:
                    if (j < 16)
                    {
                        return new BitString(new byte[] { currentIv[j] });
                    }
                    else
                    {
                        return previousOutputs[j - 16].GetDeepCopy();
                    }
                case 128:
                    if (j == 0)
                    {
                        return currentIv.GetDeepCopy();
                    }
                    else
                    {
                        return previousOutputs[previousOutputs.Count - 2].GetDeepCopy();
                    }
                default:
                    throw new ArgumentException(nameof(Shift));
            }
        }

        private BitString ConcatenateOutputsStartingAtIndex(List<BitString> previousOutputs, int startingIndex)
        {
            BitString bs = previousOutputs[startingIndex].GetMostSignificantBits(Shift);

            for (int iterator = startingIndex + 1; iterator < previousOutputs.Count; iterator++)
            {
                bs = bs.ConcatenateBits(previousOutputs[iterator].GetMostSignificantBits(Shift));
            }

            return bs;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
