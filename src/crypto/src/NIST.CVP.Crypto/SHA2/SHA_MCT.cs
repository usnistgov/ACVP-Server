using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHA_MCT : ISHA_MCT
    {
        private readonly ISHA _iSHA;
        private List<BitString> _digests;
        private int NUM_OF_RESPONSES = 100;

        public SHA_MCT(ISHA iSHA)
        {
            _iSHA = iSHA;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
         * Seed = random n bits, where n is digest size.
         * 
         * For 100 iterations (j = 0)
         *     MD[0] = MD[1] = MD[2] = Seed
         *     
         *     For 1000 iterations (i = 3)
         *         M[i] = MD[i-3] || MD[i-2] || MD[i-1]
         *         MD[i] = SHA(M[i])
         *         
         *     MD[j] = Seed = MD[1002]      (last MD from inner loop)
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, bool isSample = false)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();
            var i = 0;
            var j = 0;

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    BitString innerMessage = ResetDigestList(message);
                    BitString innerDigest = null;

                    var iterationResponse = new AlgoArrayResponse
                    {
                        Message = innerMessage
                    };

                    for (j = 0; j < 1000; j++)
                    {
                        var innerResult = _iSHA.HashMessage(function, innerMessage);
                        innerDigest = innerResult.Digest;
                        AddDigestToList(innerDigest);
                        innerMessage = GetNextMessage();
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
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private BitString ResetDigestList(BitString message)
        {
            _digests = new List<BitString>();
            _digests.Add(message);
            _digests.Add(message);
            _digests.Add(message);

            return GetNextMessage();
        }

        private BitString GetNextMessage()
        {
            return BitString.ConcatenateBits(_digests[0], BitString.ConcatenateBits(_digests[1], _digests[2]));
        }

        private void AddDigestToList(BitString newDigest)
        {
            _digests.RemoveAt(0);
            _digests.Add(newDigest);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
