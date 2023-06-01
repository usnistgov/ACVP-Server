using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT
{
    public class Sha3Mct : IShaMct
    {
        private readonly ISha _sha;
        private int NUM_OF_RESPONSES = 100;

        public Sha3Mct(ISha sha)
        {
            _sha = sha;
        }

        #region MonteCarloAlgorithm Pseudocode

        /* 
         * INPUT: A random Seed n bits long
         * {
         *    MD[0] = Seed;
         *    for (j=0; j<100; j++) {
         *        for (i=1; i<1001; i++) {
         *            M[i] = MD[i-1];
         *            MD[i] = SHA3(M[i]);
         *        }
         *        MD[0] = MD[1000];
         *        OUTPUT: MD[0]
         *    }
         * }
         */

        #endregion

        public MctResult<AlgoArrayResponse> MctHash(BitString message, bool isSample = false, MathDomain domain = null,
            int digestSize = 0, int smallestSupportedMessageLengthGreaterThanZero = 0)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var i = 0;
            var j = 0;

            var responses = new List<AlgoArrayResponse>();

            try
            {
                // Use old algorithm
                if (digestSize == 0 || domain.IsWithinDomain(digestSize))
                {
                    UseOldMctAlgo(message, responses, i, j);
                }
                // Use new algorithm
                else
                {
                    UseNewMctAlgo(message, domain, responses, i, j, digestSize,
                        smallestSupportedMessageLengthGreaterThanZero);
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

        private void UseOldMctAlgo(BitString message, List<AlgoArrayResponse> responses, int i, int j)
        {
            for (i = 0; i < NUM_OF_RESPONSES; i++)
            {
                var iterationResponse = new AlgoArrayResponse { Message = message };
                var innerMessage = message.GetDeepCopy();
                var innerDigest = new BitString(0);

                for (j = 0; j < 1000; j++)
                {
                    var innerResult = _sha.HashMessage(innerMessage);
                    innerDigest = innerResult.Digest;
                    innerMessage = innerDigest.GetDeepCopy();
                }

                iterationResponse.Digest = innerDigest;
                responses.Add(iterationResponse);
                message = innerDigest;
            }
        }
        
        #region MonteCarloNewAlgorithm Pseudocode
        /*
         *  SEED = GetRandomBitsOfLength(MinimumSupportedMessageLengthGreaterThanZero)
         *   MD[0] = SEED
         *   For 100 iterations
         *       For 1000 iterations (i = 1)x
         *           M[i] = MD[i-1];
         *           If  !SupportedMessageLengths.Contains(LEN(M[i])):
         *               M[i] = TruncateToSize(M[i], MinimumSupportedMessageLengthGreaterThanZero)
         *           MD[i] = SHA3(M[i])
         *           If MinimumSupportedMessageLengthGreaterThanZero >= digestSize:
         *               MD[i] = MD[i] || CreateZeroBitStringOfLength(MinimumSupportedMessageLengthGreaterThanZero - digestSize)
         *           Else:
         *              MD[i] = TruncateToSize(MD[i], MinimumSupportedMessageLengthGreaterThanZero)
         *       MD[0] = MD[1000]
         *       Output MD[0]
         */
        #endregion
        private void UseNewMctAlgo(BitString message, MathDomain domain, List<AlgoArrayResponse> responses, 
            int i, int j, int digestSize, int smallestSupportedMessageLengthGreaterThanZero)
        {
            for (i = 0; i < NUM_OF_RESPONSES; i++)
            {
                var iterationResponse = new AlgoArrayResponse { Message = message };
                var innerMessage = message.GetDeepCopy();
                var innerDigest = new BitString(0);

                for (j = 0; j < 1000; j++)
                {
                    if (!domain.IsWithinDomain(innerMessage.BitLength))
                    {
                        innerMessage = Sha3DerivedHelpers.TruncateMessage(innerMessage, smallestSupportedMessageLengthGreaterThanZero);
                    }
                    var innerResult = _sha.HashMessage(innerMessage);
                    innerDigest = innerResult.Digest;
                    if (innerDigest.BitLength < smallestSupportedMessageLengthGreaterThanZero)
                    {
                        innerDigest = innerResult.Digest.ConcatenateBits(BitString.Zeroes(smallestSupportedMessageLengthGreaterThanZero - digestSize));
                    } else {
                        innerDigest = Sha3DerivedHelpers.TruncateMessage(innerResult.Digest, smallestSupportedMessageLengthGreaterThanZero);
                    }
                    innerMessage = innerDigest.GetDeepCopy();
                }

                iterationResponse.Digest = innerDigest;
                responses.Add(iterationResponse);
                message = innerDigest;
            }
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
