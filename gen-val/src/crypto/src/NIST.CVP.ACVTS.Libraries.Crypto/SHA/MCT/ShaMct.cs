using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT
{
    public class ShaMct : IShaMct
    {
        private readonly ISha _sha;
        private List<BitString> _digests;
        private int NUM_OF_RESPONSES = 100;

        public ShaMct(ISha sha)
        {
            _sha = sha;
        }

        public MctResult<AlgoArrayResponse> MctHash(BitString message, bool isSample = false, MathDomain domain = null, int digestSize = 0,
           int smallestSupportedMessageLengthGreaterThanZero = 0)
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
            if (digestSize == 0 || domain.IsWithinDomain(3 * digestSize))
            {
              UseOldMctAlgo(message, responses, i, j);
            }
            // Use new algorithm
            else
            {
              UseNewMctAlgo(message, domain, responses, i, j, digestSize, smallestSupportedMessageLengthGreaterThanZero);
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
        
        /*
         * if SupportedMessageLengths.Contains(3*digestSize):
         *     SEED = GetRandomBitsOfLength(digestSize)
         *     For j = 0 to 99
         *       A = B = C = SEED
         *       For i = 0 to 999
         *           MSG = A || B || C
         *           MD = SHA(MSG)
         *           A = B
         *           B = C
         *           C = MD
         *       Output MD
         *       SEED = MD
         */
        #endregion MonteCarloAlgorithm Pseudocode
        private void UseOldMctAlgo(BitString message, List<AlgoArrayResponse> responses, int i, int j)
        {
          for (i = 0; i < NUM_OF_RESPONSES; i++)
          {
            BitString innerMessage = ResetDigestList(message);
            BitString innerDigest = null;

            var iterationResponse = new AlgoArrayResponse { Message = innerMessage };

            for (j = 0; j < 1000; j++)
            {
              var innerResult = _sha.HashMessage(innerMessage);
              innerDigest = innerResult.Digest;
              AddDigestToList(innerDigest);
              innerMessage = GetNextMessage();
            }

            iterationResponse.Digest = innerDigest;
            responses.Add(iterationResponse);
            message = innerDigest;
          }
        }
        
        #region CustomMsgLenMonteCarloAlgorithm Pseudocode
        // CASE: The IUT supports a custom range
        // if !SupportedMessageLengths.Contains(3*digestSize):
        //     SEED = GetRandomBitsOfLength(MinimumSupportedMessageLengthGreaterThanZero)
        //     For j = 0 to 99
        //         A = B = C = SEED
        //         For i = 0 to 999
        //             MSG = A || B || C
        //             If  !SupportedMessageLengths.Contains(LEN(MSG)):
        //                 MSG = TruncateToSize(MSG, MinimumSupportedMessageLengthGreaterThanZero)
        //             MD = SHA(MSG)
        //             A = B
        //             B = C
        //             If MinimumSupportedMessageLengthGreaterThanZero >= digestSize:
        //                 C = MD || CreateZeroBitStringOfLength(MinimumSupportedMessageLengthGreaterThanZero - digestSize)
        //             Else:
        //                 C = TruncateToSize(MD, MinimumSupportedMessageLengthGreaterThanZero)
        //         Output MD
        //         SEED = C
        #endregion CustomMsgLenMonteCarloAlgorithm Pseudocode
        private void UseNewMctAlgo(BitString seedMessage, MathDomain domain, List<AlgoArrayResponse> responses,
          int i, int j, int digestSize, int smallestSupportedMessageLengthGreaterThanZero)
        {
          for (i = 0; i < NUM_OF_RESPONSES; i++)
          {
            BitString innerMessage = ResetDigestList(seedMessage);
            BitString innerDigest = null;
            
            var iterationResponse = new AlgoArrayResponse { Message = innerMessage };

            for (j = 0; j < 1000; j++)
            {
              if (!domain.IsWithinDomain(innerMessage.BitLength))
              {
                innerMessage = TruncateMessage(innerMessage, smallestSupportedMessageLengthGreaterThanZero);
              }
              
              var innerResult = _sha.HashMessage(innerMessage);
              innerDigest = innerResult.Digest;
              
              _digests[0] = _digests[1];
              _digests[1] = _digests[2];
              
              if (innerResult.Digest.BitLength < smallestSupportedMessageLengthGreaterThanZero)
              {
                _digests[2] =
                  innerResult.Digest.ConcatenateBits(
                    BitString.Zeroes(smallestSupportedMessageLengthGreaterThanZero - digestSize));
              } else {
                _digests[2] = TruncateMessage(innerResult.Digest, smallestSupportedMessageLengthGreaterThanZero);
              }
              
              innerMessage = GetNextMessage();
            }
            
            iterationResponse.Digest = innerDigest;
            responses.Add(iterationResponse);
            
            seedMessage = _digests[2];
          }
        }

        private BitString TruncateMessage(BitString innerMessage, int customMinMsgLength)
        {
          return innerMessage.Substring(0, customMinMsgLength);
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

        private void AddDigestToList(BitString newDigest)
        {
            _digests.RemoveAt(0);
            _digests.Add(newDigest);
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
