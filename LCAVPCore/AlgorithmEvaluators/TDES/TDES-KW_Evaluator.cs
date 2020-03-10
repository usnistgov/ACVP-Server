using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.TDES;

namespace LCAVPCore.AlgorithmEvaluators.TDES
{
	public class TDES_KW_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public TDES_KW_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new TDES_KW_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Test enabling is nested - need action (encrypt/decrypt) and cipher direction (FWD/INV)
				if (Options.GetValue("TKW_AE") == "True")
				{
					if (Options.GetValue("TKW_FWD_CIPHER") == "True" && !results.EncryptPassed) algoResult.AddFailure("Encrypt failed");
					if (Options.GetValue("TKW_INV_CIPHER") == "True" && !results.EncryptInversePassed) algoResult.AddFailure("Encrypt Inverse failed");
				}

				if (Options.GetValue("TKW_AD") == "True")
				{
					if (Options.GetValue("TKW_FWD_CIPHER") == "True" && !results.DecryptPassed) algoResult.AddFailure("Decrypt failed");
					if (Options.GetValue("TKW_INV_CIPHER") == "True" && !results.DecryptInversePassed) algoResult.AddFailure("Decrypt Inverse failed");
				}
			}

			return algoResult;
		}
	}
}