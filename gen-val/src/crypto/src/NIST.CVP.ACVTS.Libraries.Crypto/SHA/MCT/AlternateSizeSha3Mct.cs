using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT
{
    public class AlternateSizeSha3Mct : IShaMct
    {
        private readonly ISha _sha;
        private int NUM_OF_RESPONSES = 100;
        
        public AlternateSizeSha3Mct(ISha sha)
        {
            _sha = sha;
        }

        #region AlternateMonteCarloAlgorithm Pseudocode
        /*
         *
         * MD[0] = SEED
         * For 100 iterations
         *     For i = 1 to 1000
         *         MSG = MD[i-1];
         *         if LEN(MSG) >= LEN(SEED):
         *             MSG = leftmost LEN(SEED) bits of MSG
         *         else:
         *             MSG = MSG || LEN(SEED) - LEN(MSG) 0 bits
         *         MD[i] = SHA3(MSG)            
         *     MD[0] = MD[1000]
         *     Output MD[0]
         * 
         */

        #endregion AlternateMonteCarloAlgorithm Pseudocode

        public MctResult<AlgoArrayResponse> MctHash(BitString message, bool isSample = false, MathDomain domain = null)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var i = 0;
            var j = 0;
            var seedLength = message.BitLength;

            var responses = new List<AlgoArrayResponse>();

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var iterationResponse = new AlgoArrayResponse { Message = message };
                    var innerMessage = message.GetDeepCopy();
                    var innerDigest = new BitString(0);

                    for (j = 0; j < 1000; j++)
                    {
                        if (innerMessage.BitLength >= seedLength)
                        {
                            innerMessage = innerMessage.GetMostSignificantBits(seedLength);
                        }
                        else
                        {
                            innerMessage.ConcatenateBits(BitString.Zeroes(seedLength - innerMessage.BitLength));
                        }
                        
                        var innerResult = _sha.HashMessage(innerMessage);
                        innerDigest = innerResult.Digest;
                        
                        innerMessage = innerDigest.GetDeepCopy();
                    }

                    iterationResponse.Digest = innerDigest;
                    responses.Add(iterationResponse);
                    message = innerDigest;
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MctResult<AlgoArrayResponse>(ex.Message);
            }

            return new MctResult<AlgoArrayResponse>(responses);
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
