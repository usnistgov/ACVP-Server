using System;
using System.Collections;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Crypto.CSHAKE
{
    public class CSHAKE_MCT : ICSHAKE_MCT
    {
        private readonly ICSHAKE _iCSHAKE;
        private int NUM_OF_RESPONSES = 100;

        public CSHAKE_MCT(ICSHAKE iCSHAKE)
        {
            _iCSHAKE = iCSHAKE;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
         * INPUT: The initial Msg of 128 bits long
         * 
         * BitsToString(bits) { 
         *     string = "";
         *     foreach byte in bits {
         *         string = string + ASCII((byte % 26) + 65); 
         *     }
         * }
         * 
         * Initial Outputlen = (floor(maxoutlen/8) )*8
         * Initial FunctionName = ""
         * Initial Customization = ""
         * //makes maxoutlen a multiple of 8 and remains within the range specified.
         * 
         * {
         *     Output0 = Msg;
         *     for (j=0; j<100; j++) {
         *         for (i=1; i<1001; i++) {
         *             M[i] = 128 leftmost bits of Output[i-1];
         *             Output[i] = CSHAKE(M[i],Outputlen,FunctionName,Customization);
         *             If (i == 1000){
         *                 Outputlen[j] = Outputlen;
         *             }
         *             Rightmost_Output_bits = rightmost 16 bits of Output[i];
         *             Leftmost_Output_bits = leftmost 16 bits of Output[i];
         *             Range = (maxoutbytes – minoutbytes + 1);
         *             Outputlen = minoutbytes + (Rightmost_Output_bits mod Range);
         *             Customization = BitsToString(M[i] || Rightmost_Output_bits);
         *         }
         *         Output[j] = Output[1000];
         *         OUTPUT: Outputlen[j], Output[j]
         *     }
         * }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain domain, bool isSample)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponseWithCustomization>();
            var i = 0;
            var j = 0;
            var min = domain.GetDomainMinMax().Minimum;
            var max = domain.GetDomainMinMax().Maximum;
            var minBytes = min / 8;
            var maxBytes = max / 8;

            var outputLen = (int)System.Math.Floor((double)max / 8) * 8;
            var functionName = "";
            var customization = "";
            var range = (max - min) + 8;
            var innerMessage = message.GetDeepCopy();

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerDigest = new BitString(0);
                    var iterationResponse = new AlgoArrayResponseWithCustomization() { };
                    iterationResponse.Message = innerMessage;
                    iterationResponse.Customization = customization;

                    for (j = 0; j < 1000; j++)
                    {
                        // Might not have 128 bits to pull from so we pad with 0                        
                        innerMessage = BitString.ConcatenateBits(innerMessage, BitString.Zeroes(128));
                        innerMessage = BitString.MSBSubstring(innerMessage, 0, 128);
                        function.DigestLength = outputLen;
                        function.FunctionName = functionName;
                        function.Customization = customization;

                        var innerResult = _iCSHAKE.HashMessage(function, innerMessage);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBitString = BitString.Substring(innerDigest, 0, 16);
                        var rightmostBits = rightmostBitString.Bits;

                        outputLen = min + (8 * GetIntFromBits(rightmostBits)) % range;
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
                return new MCTResult<AlgoArrayResponseWithCustomization>($"{ex.Message}; {outputLen}");
            }

            return new MCTResult<AlgoArrayResponseWithCustomization>(responses);
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
    }
}
