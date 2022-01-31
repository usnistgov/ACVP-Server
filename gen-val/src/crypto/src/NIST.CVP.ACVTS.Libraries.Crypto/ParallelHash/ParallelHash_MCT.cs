using System;
using System.Collections;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash
{
    public class ParallelHash_MCT : IParallelHash_MCT
    {
        private readonly IParallelHash _iParallelHash;
        private bool _hexCustomization;
        private int NUM_OF_RESPONSES = 100;

        public ParallelHash_MCT(IParallelHash iParallelHash)
        {
            _iParallelHash = iParallelHash;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
            INPUT: The initial Msg is the length of the digest size

            MCT(Msg, MaxOutLen, MinOutLen, OutLenIncrement, MaxBlockSize, MinBlockSize)
            {
              Range = (MaxOutLen – MinOutLen + 1);
              OutputLen = MaxOutLen;
              BlockRange = (MaxBlockSize – MinBlockSize + 1);
              BlockSize = MinBlockSize;
              Customization = "";

              Output[0] = Msg;
              for (j = 0; j < 100; j++) 
              {
                for (i = 1; i < 1001; i++) 
                {
                  InnerMsg = Left(Output[i-1] || ZeroBits(128), 128);
                  Output[i] = ParallelHash(InnerMsg, OutputLen, BlockSize, FunctionName, Customization);
                  Rightmost_Output_bits = Right(Output[i], 16);
                  OutputLen = MinOutLen + (floor((Rightmost_Output_bits % Range) / OutLenIncrement) * OutLenIncrement);
                  BlockSize = MinBlockSize + Right(Rightmost_Output_bits, 8) % BlockRange;
                  Customization = BitsToString(InnerMsg || Rightmost_Output_bits);
                }
              
                OutputJ[j] = Output[1000];
              }

              return OutputJ;
            }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MctResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain outputLength, MathDomain blockSizeDomain, bool hexCustomization, bool isSample)
        {
            _hexCustomization = hexCustomization;
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponseWithCustomization>();
            var i = 0;
            var j = 0;
            var min = outputLength.GetDomainMinMax().Minimum;
            var max = outputLength.GetDomainMinMax().Maximum;
            var increment = outputLength.GetDomainMinMax().Increment;
            var minBlockSize = blockSizeDomain.GetDomainMinMax().Minimum;
            var maxBlockSize = blockSizeDomain.GetDomainMinMax().Maximum;

            var outputLen = max;
            var blockSize = minBlockSize;
            var blockSizeRange = (maxBlockSize - minBlockSize) + 1;
            var customization = "";
            var range = (max - min) + 1;
            var innerMessage = message.GetDeepCopy();

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerDigest = new BitString(0);
                    var iterationResponse = new AlgoArrayResponseWithCustomization() { };
                    iterationResponse.Message = innerMessage;
                    iterationResponse.Customization = customization;
                    iterationResponse.BlockSize = blockSize;

                    for (j = 0; j < 1000; j++)
                    {
                        // Might not have 128 bits to pull from so we pad with 0                        
                        innerMessage = BitString.ConcatenateBits(innerMessage, BitString.Zeroes(128))
                            .GetMostSignificantBits(128);

                        function.DigestLength = outputLen;

                        var innerResult = _iParallelHash.HashMessage(function, innerMessage, blockSize, customization);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBitString = innerDigest.GetLeastSignificantBits(16);

                        var rightmostBits = rightmostBitString.Bits;

                        outputLen = min + (int)System.Math.Floor((double)(rightmostBits.ToInt() % range) / increment) * increment;
                        blockSize = minBlockSize + rightmostBitString.GetLeastSignificantBits(8).Bits.ToInt() % blockSizeRange;
                        customization = GetStringFromBytes(BitString.ConcatenateBits(innerMessage, rightmostBitString).ToBytes());

                        innerMessage = innerDigest.GetDeepCopy();
                    }

                    iterationResponse.Digest = innerDigest.GetDeepCopy();
                    responses.Add(iterationResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MctResult<AlgoArrayResponseWithCustomization>($"{ex.Message}; {outputLen}");
            }

            return new MctResult<AlgoArrayResponseWithCustomization>(responses);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private string GetStringFromBytes(byte[] bytes)
        {
            var result = "";
            if (_hexCustomization)
            {
                result = new BitString(bytes).ToHex();
            }
            else
            {
                foreach (var num in bytes)
                {
                    result += System.Text.Encoding.ASCII.GetString(new byte[] { (byte)((num % 26) + 65) });
                }
            }

            return result;
        }
    }
}
