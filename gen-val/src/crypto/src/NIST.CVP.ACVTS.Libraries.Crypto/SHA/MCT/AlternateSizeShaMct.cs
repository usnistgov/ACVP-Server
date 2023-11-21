using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT
{
    public class AlternateSizeShaMct : IShaMct
    {
        private readonly ISha _sha;
        private List<BitString> _digests;
        
        private int NUM_OF_RESPONSES = 100;

        public AlternateSizeShaMct(ISha sha)
        {
            _sha = sha;
        }
        
        #region AlternateMonteCarloAlgorithm Pseudocode
        /*
         *
         * CASE: The IUT supports a custom range
         * For j = 0 to 99
         *     A = B = C = SEED
         *     For i = 0 to 999
         *         MSG = A || B || C
         *         if LEN(MSG) >= LEN(SEED):
         *             MSG = leftmost LEN(SEED) bits of MSG
         *         else:
         *             MSG = MSG || LEN(SEED) - LEN(MSG) 0 bits
         *     MD = SHA(MSG)
         *     A = B
         *     B = C
         *     C = MD
         * Output MD
         * SEED = MD
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
              BitString innerMessage = ResetDigestList(message);
              BitString innerDigest = null;
            
              var iterationResponse = new AlgoArrayResponse { Message = innerMessage };

              for (j = 0; j < 1000; j++)
              {
                if (innerMessage.BitLength >= seedLength)
                {
                  innerMessage = innerMessage.GetMostSignificantBits(seedLength);
                }
                else
                {
                  innerMessage = innerMessage.ConcatenateBits(BitString.Zeroes(seedLength - innerMessage.BitLength));
                }

                var innerResult = _sha.HashMessage(innerMessage); 
                innerDigest = innerResult.Digest;
              
                _digests[0] = _digests[1];
                _digests[1] = _digests[2];
                _digests[2] = innerDigest;
              
                innerMessage = GetNextMessage();
              }
            
              iterationResponse.Digest = innerDigest;
              responses.Add(iterationResponse);
            
              message = _digests[2];
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

        private BitString ResetDigestList(BitString message)
        {
          _digests = new List<BitString> { message, message, message };

          return GetNextMessage();
        }

        private BitString GetNextMessage()
        {
          return BitString.ConcatenateBits(_digests[0],
            BitString.ConcatenateBits(_digests[1], _digests[2]));
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
