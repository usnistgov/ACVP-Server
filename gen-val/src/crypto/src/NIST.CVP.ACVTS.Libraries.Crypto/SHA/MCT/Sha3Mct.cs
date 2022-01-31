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

        public MctResult<AlgoArrayResponse> MctHash(BitString message, MathDomain domain = null, bool isSample = false)
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
