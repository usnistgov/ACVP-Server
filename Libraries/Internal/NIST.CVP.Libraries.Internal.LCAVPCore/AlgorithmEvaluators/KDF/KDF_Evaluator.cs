using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KDF;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.KDF
{
	public class KDF_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public KDF_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new KDF_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Counter
				if (Options.GetValue("KDF108_CTRMode") == "True")
				{
					if (Options.GetValue("KDF108_CTRModePRFCMACAES128") == "True" && !results.Counter_CMAC_AES128Passed) algoResult.AddFailure("Counter CMAC_AES128 failed");
					if (Options.GetValue("KDF108_CTRModePRFCMACAES192") == "True" && !results.Counter_CMAC_AES192Passed) algoResult.AddFailure("Counter CMAC_AES192 failed");
					if (Options.GetValue("KDF108_CTRModePRFCMACAES256") == "True" && !results.Counter_CMAC_AES256Passed) algoResult.AddFailure("Counter CMAC_AES256 failed");
					if (Options.GetValue("KDF108_CTRModePRFCMACTDES2") == "True" && !results.Counter_CMAC_TDES2Passed) algoResult.AddFailure("Counter CMAC_TDES2 failed");
					if (Options.GetValue("KDF108_CTRModePRFCMACTDES3") == "True" && !results.Counter_CMAC_TDES3Passed) algoResult.AddFailure("Counter CMAC_TDES3 failed");
					if (Options.GetValue("KDF108_CTRModePRFHMACSHA1") == "True" && !results.Counter_HMAC_SHA1Passed) algoResult.AddFailure("Counter HMAC_SHA1 failed");
					if (Options.GetValue("KDF108_CTRModePRFHMACSHA224") == "True" && !results.Counter_HMAC_SHA224Passed) algoResult.AddFailure("Counter HMAC_SHA224 failed");
					if (Options.GetValue("KDF108_CTRModePRFHMACSHA256") == "True" && !results.Counter_HMAC_SHA256Passed) algoResult.AddFailure("Counter HMAC_SHA256 failed");
					if (Options.GetValue("KDF108_CTRModePRFHMACSHA384") == "True" && !results.Counter_HMAC_SHA384Passed) algoResult.AddFailure("Counter HMAC_SHA384 failed");
					if (Options.GetValue("KDF108_CTRModePRFHMACSHA512") == "True" && !results.Counter_HMAC_SHA512Passed) algoResult.AddFailure("Counter HMAC_SHA512 failed");
				}

				//Double Pipeline
				if (Options.GetValue("KDF108_PipelineMode") == "True")
				{
					if (Options.GetValue("KDF108_PIPEModePRFCMACAES128") == "True" && !results.Pipeline_CMAC_AES128Passed) algoResult.AddFailure("Pipeline CMAC_AES128 failed");
					if (Options.GetValue("KDF108_PIPEModePRFCMACAES192") == "True" && !results.Pipeline_CMAC_AES192Passed) algoResult.AddFailure("Pipeline CMAC_AES192 failed");
					if (Options.GetValue("KDF108_PIPEModePRFCMACAES256") == "True" && !results.Pipeline_CMAC_AES256Passed) algoResult.AddFailure("Pipeline CMAC_AES256 failed");
					if (Options.GetValue("KDF108_PIPEModePRFCMACTDES2") == "True" && !results.Pipeline_CMAC_TDES2Passed) algoResult.AddFailure("Pipeline CMAC_TDES2 failed");
					if (Options.GetValue("KDF108_PIPEModePRFCMACTDES3") == "True" && !results.Pipeline_CMAC_TDES3Passed) algoResult.AddFailure("Pipeline CMAC_TDES3 failed");
					if (Options.GetValue("KDF108_PIPEModePRFHMACSHA1") == "True" && !results.Pipeline_HMAC_SHA1Passed) algoResult.AddFailure("Pipeline HMAC_SHA1 failed");
					if (Options.GetValue("KDF108_PIPEModePRFHMACSHA224") == "True" && !results.Pipeline_HMAC_SHA224Passed) algoResult.AddFailure("Pipeline HMAC_SHA224 failed");
					if (Options.GetValue("KDF108_PIPEModePRFHMACSHA256") == "True" && !results.Pipeline_HMAC_SHA256Passed) algoResult.AddFailure("Pipeline HMAC_SHA256 failed");
					if (Options.GetValue("KDF108_PIPEModePRFHMACSHA384") == "True" && !results.Pipeline_HMAC_SHA384Passed) algoResult.AddFailure("Pipeline HMAC_SHA384 failed");
					if (Options.GetValue("KDF108_PIPEModePRFHMACSHA512") == "True" && !results.Pipeline_HMAC_SHA512Passed) algoResult.AddFailure("Pipeline HMAC_SHA512 failed");
				}

				//Feedback
				if (Options.GetValue("KDF108_FeedbackMode") == "True")
				{
					if (Options.GetValue("KDF108_FDBKModePRFCMACAES128") == "True" && !results.Feedback_CMAC_AES128Passed) algoResult.AddFailure("FeedbackCMAC_AES128 failed");
					if (Options.GetValue("KDF108_FDBKModePRFCMACAES192") == "True" && !results.Feedback_CMAC_AES192Passed) algoResult.AddFailure("FeedbackCMAC_AES192 failed");
					if (Options.GetValue("KDF108_FDBKModePRFCMACAES256") == "True" && !results.Feedback_CMAC_AES256Passed) algoResult.AddFailure("FeedbackCMAC_AES256 failed");
					if (Options.GetValue("KDF108_FDBKModePRFCMACTDES2") == "True" && !results.Feedback_CMAC_TDES2Passed) algoResult.AddFailure("FeedbackCMAC_TDES2 failed");
					if (Options.GetValue("KDF108_FDBKModePRFCMACTDES3") == "True" && !results.Feedback_CMAC_TDES3Passed) algoResult.AddFailure("FeedbackCMAC_TDES3 failed");
					if (Options.GetValue("KDF108_FDBKModePRFHMACSHA1") == "True" && !results.Feedback_HMAC_SHA1Passed) algoResult.AddFailure("FeedbackHMAC_SHA1 failed");
					if (Options.GetValue("KDF108_FDBKModePRFHMACSHA224") == "True" && !results.Feedback_HMAC_SHA224Passed) algoResult.AddFailure("FeedbackHMAC_SHA224 failed");
					if (Options.GetValue("KDF108_FDBKModePRFHMACSHA256") == "True" && !results.Feedback_HMAC_SHA256Passed) algoResult.AddFailure("FeedbackHMAC_SHA256 failed");
					if (Options.GetValue("KDF108_FDBKModePRFHMACSHA384") == "True" && !results.Feedback_HMAC_SHA384Passed) algoResult.AddFailure("FeedbackHMAC_SHA384 failed");
					if (Options.GetValue("KDF108_FDBKModePRFHMACSHA512") == "True" && !results.Feedback_HMAC_SHA512Passed) algoResult.AddFailure("FeedbackHMAC_SHA512 failed");
				}
			}

			return algoResult;
		}
	}
}
