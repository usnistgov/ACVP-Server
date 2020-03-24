using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class IKEv1_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public IKEv1_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new IKEv1_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("KDF_800_135_IKEv1_DigitalSignatureAuthentication") == "True" && !results.DigitalSignatureAuthenticationPassed) algoResult.AddFailure("DigitalSignatureAuthentication failed");
				if (Options.GetValue("KDF_800_135_IKEv1_PublicKeyEncryptionAuthentication") == "True" && !results.PublicKeyEncryptionAuthenticationPassed) algoResult.AddFailure("PublicKeyEncryptionAuthentication failed");
				if (Options.GetValue("KDF_800_135_IKEv1_PreSharedKeyAuthentication") == "True" && !results.PresharedKeyAuthenticationPassed) algoResult.AddFailure("PreSharedKeyAuthentication failed");
			}

			return algoResult;
		}
	}
}
