using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public class LmsMct : ILmsMct
    {
        private readonly IHssFactory _hssFactory;
        private int NUM_OF_RESPONSES = 100;

        public LmsMct(IHssFactory hssFactory)
        {
            _hssFactory = hssFactory;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
         * INPUT: The initial Msg of 256 bits long
         *        LmsParams
         *        SEED
         *        RootI
         * 
         * {
         *     lmsKeyPair = GenerateLmsKeyPair(SEED, RootI, Params);
         *     Output0 = Msg;
         *     for (j=0; j<100; j++) {
         *         for (i=1; i<(isSample ? 101 : 1001); i++) {
         *             M[i] = leftmost bits indexed from 96 to 352;
         *             if (lmsKeyPair is expired) {
         *                 lmsKeyPair = GenerateLmsKeyPair(M[i], 128 left most bits of M[i], Params);
         *             }
         *             Output[i] = LmsSignature(M[i],lmsKeyPair);
         *             lmsKeyPair = UpdateLmsKeyPair(lmsKeyPair);
         *         }
         *         Output[j] = Output[1000];
         *         OUTPUT: Output[j]
         *     }
         * }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public async Task<MCTResult<AlgoArrayResponse>> MCTHashAsync(LmsType[] lmsTypes, LmotsType[] lmotsTypes,
            BitString seed, BitString rootI, BitString message, bool isSample)
        {
            var hss = _hssFactory.GetInstance(lmsTypes.Length, lmsTypes, lmotsTypes, EntropyProviderTypes.Testable, seed, rootI);

            var keyPair = await hss.GenerateHssKeyPairAsync();

            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponse>();
            var i = 0;
            var j = 0;

            var innerMessage = message.GetDeepCopy();

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerSignature = new BitString(0);
                    var iterationResponse = new AlgoArrayResponse() { };
                    iterationResponse.Message = innerMessage;

                    for (j = 0; j < (isSample ? 100 : 1000); j++)
                    {
                        if (keyPair.Expired)
                        {
                            hss = _hssFactory.GetInstance(lmsTypes.Length, lmsTypes, lmotsTypes,
                                EntropyProviderTypes.Testable, innerMessage, innerMessage.MSBSubstring(0, 128));
                            keyPair = await hss.GenerateHssKeyPairAsync();
                        }

                        innerSignature = (await hss.GenerateHssSignatureAsync(innerMessage, keyPair)).Signature;

                        keyPair = await hss.UpdateKeyPairOneStepAsync(keyPair);

                        innerMessage = innerSignature.MSBSubstring(96, 256);
                    }

                    iterationResponse.Signature = innerSignature.GetDeepCopy();
                    responses.Add(iterationResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResult<AlgoArrayResponse>($"{ex.Message}");
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
