using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TupleHash
{
    public class TupleHash_MCT : ITupleHash_MCT
    {
        private readonly ITupleHash _iTupleHash;
        private bool _hexCustomization;
        private int NUM_OF_RESPONSES = 100;

        public TupleHash_MCT(ITupleHash iTupleHash)
        {
            _iTupleHash = iTupleHash;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
            INPUT: The initial Single-Tuple of a random length between 0 and 65536 bits.

            MCT(Tuple, MaxOutLen, MinOutLen, OutLenIncrement)
            {
              Range = (MaxOutLen – MinOutLen + 1);
              OutputLen = MaxOutLen;
              Customization = "";

              T[0][0] = Tuple;
              
              for (j = 0; j < 100; j++) 
              {
                for (i = 1; i < 1001; i++) 
                {
                  workingBits = Left(T[i-1][0] || ZeroBits(288), 288);
                  tupleSize = Left(workingBits, 3) % 4 + 1; // never more than 4 tuples to a round
                  for (k = 0; k < tupleSize; k++) 
                  {
                    T[i][k] = Substring of workingBits from (k * 288 / tupleSize) to ((k+1) * 288 / tupleSize - 1);
                  }
                  Output[i] = TupleHash(T[i], OutputLen, Customization);
                  Rightmost_Output_bits = Right(Output[i], 16);
                  OutputLen = MinOutLen + (floor((Rightmost_Output_bits % Range) / OutLenIncrement) * OutLenIncrement);
                  Customization = BitsToString(T[i][0] || Rightmost_Output_bits);
                }
              
                OutputJ[j] = Output[1000];
              }
              
              return OutputJ;
            }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResultTuple<AlgoArrayResponse> MCTHash(HashFunction function, IEnumerable<BitString> tuple, MathDomain domain, bool hexCustomization, bool isSample)
        {
            _hexCustomization = hexCustomization;
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponse>();
            var i = 0;
            var j = 0;
            var min = domain.GetDomainMinMax().Minimum;
            var max = domain.GetDomainMinMax().Maximum;
            var increment = domain.GetDomainMinMax().Increment;
            var minBytes = min / 8;
            var maxBytes = max / 8;

            var outputLen = max;
            var customization = "";
            var range = (max - min) + 1;
            var innerTuple = GetDeepCopy(tuple);

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerDigest = new BitString(0);
                    var iterationResponse = new AlgoArrayResponse() { };
                    iterationResponse.Tuple = innerTuple;
                    iterationResponse.Customization = customization;

                    for (j = 0; j < 1000; j++)
                    {
                        // Might not have 144 bits to pull from so we pad with 0  
                        var innerBitString = BitString.ConcatenateBits(innerTuple.ElementAt(0), BitString.Zeroes(288))
                            .GetMostSignificantBits(288);
                        var innerTupleSize = innerBitString.GetMostSignificantBits(3).Bits.ToInt() % 4 + 1;
                        innerTuple = new List<BitString>();
                        for (int k = 0; k < innerTupleSize; k++)
                        {
                            innerTuple.Add(BitString.MSBSubstring(innerBitString, k * 288 / innerTupleSize, 288 / innerTupleSize));
                        }

                        function.DigestLength = outputLen;

                        var innerResult = _iTupleHash.HashMessage(function, innerTuple, customization);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBitString = BitString.Substring(innerDigest, 0, 16);
                        var rightmostBits = rightmostBitString.Bits;

                        outputLen = min + (int)System.Math.Floor((double)(rightmostBits.ToInt() % range) / increment) * increment;
                        customization = GetStringFromBytes(BitString.ConcatenateBits(innerTuple.ElementAt(0), rightmostBitString).ToBytes());

                        innerTuple = new List<BitString>();
                        innerTuple.Add(innerDigest.GetDeepCopy());
                    }

                    iterationResponse.Digest = innerDigest.GetDeepCopy();
                    responses.Add(iterationResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResultTuple<AlgoArrayResponse>($"{ex.Message}; {outputLen}");
            }

            return new MCTResultTuple<AlgoArrayResponse>(responses);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }

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

        private List<BitString> GetDeepCopy(IEnumerable<BitString> tuple)
        {
            var copy = new List<BitString>();
            foreach (var bitString in tuple)
            {
                copy.Add(bitString.GetDeepCopy());
            }
            return copy;
        }
    }
}
