using System;
using System.Collections;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT
{
    public class ShakeMct : IShaMct
    {
        private readonly ISha _sha;
        private int NUM_OF_RESPONSES = 100;

        public ShakeMct(ISha sha)
        {
            _sha = sha;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
         * INPUT: The initial Msg of 128 bits long
         * 
         * Initial Outputlen = (floor(maxoutlen/8) )*8
         * //makes maxoutlen a multiple of 8 and remains within the range specified.
         * 
         * {
         *     Output0 = Msg;
         *     for (j=0; j<100; j++) {
         *         for (i=1; i<1001; i++) {
         *             M[i] = 128 leftmost bits of Output[i-1];
         *             Output[i] = SHAKE(M[i],Outputlen);
         *             If (i == 1000){
         *                 Outputlen[j] = Outputlen;
         *             }
         *             Rightmost_Output_bits = rightmost 16 bits of Output[i];
         *             Range = (maxoutbytes – minoutbytes + 1);
         *             Outputlen = minoutbytes + (Rightmost_Output_bits mod Range);
         *         }
         *         Output[j] = Output[1000];
         *         OUTPUT: Outputlen[j], Output[j]
         *     }
         * }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MctResult<AlgoArrayResponse> MctHash(BitString message, bool isSample = false, MathDomain domain = null,
            int digestSize = 0, int smallestSupportedMessageLengthGreaterThanZero = 0)
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
            var range = (max - min) + 8;
            //var range = (max - min) + min;

            var innerMessage = message.GetDeepCopy();

            // Might not have 128 bits to pull from so we pad with 0                        
            innerMessage = BitString.ConcatenateBits(innerMessage, BitString.Zeroes(128));
            innerMessage = BitString.MSBSubstring(innerMessage, 0, 128);

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerDigest = new BitString(0);
                    var iterationResponse = new AlgoArrayResponse() { };
                    iterationResponse.Message = innerMessage;

                    for (j = 0; j < 1000; j++)
                    {
                        var innerResult = _sha.HashMessage(innerMessage, outputLen);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBits = BitString.Substring(innerDigest, 0, 16).Bits;

                        outputLen = min + (8 * GetIntFromBits(rightmostBits)) % range;

                        innerMessage = innerDigest.GetDeepCopy();
                        // Might not have 128 bits to pull from so we pad with 0                        
                        innerMessage = BitString.ConcatenateBits(innerMessage, BitString.Zeroes(128));
                        innerMessage = BitString.MSBSubstring(innerMessage, 0, 128);

                    }

                    iterationResponse.Digest = innerDigest.GetDeepCopy();
                    responses.Add(iterationResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MctResult<AlgoArrayResponse>($"{ex.Message}; {outputLen}");
            }

            return new MctResult<AlgoArrayResponse>(responses);
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();

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
    }
}
