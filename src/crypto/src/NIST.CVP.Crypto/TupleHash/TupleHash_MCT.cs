using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Crypto.TupleHash
{
    public class TupleHash_MCT : ITupleHash_MCT
    {
        private readonly ITupleHash _iTupleHash;
        private int NUM_OF_RESPONSES = 100;

        public TupleHash_MCT(ITupleHash iTupleHash)
        {
            _iTupleHash = iTupleHash;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
         * INPUT: The initial Single-Tuple of 144 bits long
         * 
         * BitsToString(bits) { 
         *     string = "";
         *     foreach byte in bits {
         *         string = string + ASCII((byte % 26) + 65); 
         *     }
         * }
         * 
         * Initial Outputlen = (floor(maxoutlen/8) )*8
         * Initial Customization = ""
         * //makes maxoutlen a multiple of 8 and remains within the range specified.
         * 
         * {
         *     Output0 = Msg;
         *     for (j=0; j<100; j++) {
         *         for (i=1; i<1001; i++) {
         *             leftbits = 144 leftmost bits of Output[i-1];
         *             tupleSize = 3 leftmost bits of leftbits + 1;
         *             for (k=0; k<tupleSize; k++) {
         *                 T[i][k] = Substring of leftbits from k * 144 / tupleSize to (k+1) * 144 / tupleSize - 1;
         *             }
         *             Output[i] = TupleHash(T[i],Outputlen,Customization);
         *             If (i == 1000){
         *                 Outputlen[j] = Outputlen;
         *             }
         *             Rightmost_Output_bits = rightmost 16 bits of Output[i];
         *             Range = (maxoutbytes – minoutbytes + 1);
         *             Outputlen = minoutbytes + (Rightmost_Output_bits mod Range);
         *             Customization = BitsToString(T[i][0] || Rightmost_Output_bits);
         *         }
         *         Output[j] = Output[1000];
         *         OUTPUT: Outputlen[j], Output[j]
         *     }
         * }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResultTuple<AlgoArrayResponse> MCTHash(HashFunction function, IEnumerable<BitString> tuple, MathDomain domain, bool isSample)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponse>();
            var i = 0;
            var j = 0;
            var min = domain.GetDomainMinMax().Minimum;
            var max = domain.GetDomainMinMax().Maximum;
            var minBytes = min / 8;
            var maxBytes = max / 8;

            var outputLen = (int)System.Math.Floor((double)max / 8) * 8;
            var customization = "";
            var range = (max - min) + 8;
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
                        var innerBitString = BitString.ConcatenateBits(innerTuple.ElementAt(0), BitString.Zeroes(144));
                        innerBitString = BitString.MSBSubstring(innerBitString, 0, 144);
                        var innerTupleSize = GetIntFromBits(BitString.MSBSubstring(innerBitString, 0, 3).Bits) % 4 + 1;
                        innerTuple = new List<BitString>();
                        for (int k = 0; k < innerTupleSize; k++)
                        {
                            innerTuple.Add(BitString.MSBSubstring(innerBitString, k * 144 / innerTupleSize, 144 / innerTupleSize));
                        }
                        
                        function.DigestSize = outputLen;
                        function.Customization = customization;

                        var innerResult = _iTupleHash.HashMessage(function, innerTuple);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBitString = BitString.Substring(innerDigest, 0, 16);
                        var rightmostBits = rightmostBitString.Bits;

                        outputLen = min + (8 * GetIntFromBits(rightmostBits)) % range;
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

        private int GetIntFromBits(BitArray bits)
        {
            var value = 0;
            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    value += 1 << i;
                }
            }

            return value;
        }

        private string GetStringFromBytes(byte[] bytes)
        {
            var result = "";
            foreach (var num in bytes)
            {
                result += System.Text.Encoding.ASCII.GetString(new byte[] { (byte)((num % 26) + 65) });
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
