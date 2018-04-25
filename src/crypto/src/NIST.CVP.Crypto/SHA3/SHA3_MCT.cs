using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Crypto.SHA3
{
    public class SHA3_MCT : ISHA3_MCT
    {
        private readonly ISHA3 _iSHA3;
        private int NUM_OF_RESPONSES = 100;

        public SHA3_MCT(ISHA3 iSHA3)
        {
            _iSHA3 = iSHA3;
        }

        #region MonteCarloAlgorithm Pseudocode
        //INPUT: A random Seed n bits long
        //{
        //    MD[0] = Seed;
        //    for (j=0; j<100; j++) {
        //        for (i=1; i<1001; i++) {
        //            M[i] = MD[i-1];
        //            MD[i] = SHA3(M[i]);
        //        }
        //        MD[0] = MD[1000];
        //        OUTPUT: MD[0]
        //    }
        //}
        #endregion

        public MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, MathDomain domain = null, bool isSample = false)
        {
            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponse>();
            var i = 0;
            var j = 0;

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var iterationResponse = new AlgoArrayResponse {Message = message};
                    var innerMessage = message.GetDeepCopy();
                    var innerDigest = new BitString(0);

                    for (j = 0; j < 1000; j++)
                    {
                        var innerResult = _iSHA3.HashMessage(function, innerMessage);
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
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
