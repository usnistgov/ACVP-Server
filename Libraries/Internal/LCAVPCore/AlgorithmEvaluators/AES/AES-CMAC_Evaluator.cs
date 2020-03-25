using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_CMAC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_CMAC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_CMAC_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("CMACGen_AES") == "True")
				{
					if (Options.GetValue("CMACGen_AES128") == "True" && !results.Generate128Passed) algoResult.AddFailure("Generate128 failed");
					if (Options.GetValue("CMACGen_AES192") == "True" && !results.Generate192Passed) algoResult.AddFailure("Generate192 failed");
					if (Options.GetValue("CMACGen_AES256") == "True" && !results.Generate256Passed) algoResult.AddFailure("Generate256 failed");
				}

				if (Options.GetValue("CMACVer_AES") == "True")
				{
					if (Options.GetValue("CMACVer_AES128") == "True" && !results.Verify128Passed) algoResult.AddFailure("Verify128 failed");
					if (Options.GetValue("CMACVer_AES192") == "True" && !results.Verify192Passed) algoResult.AddFailure("Verify192 failed");
					if (Options.GetValue("CMACVer_AES256") == "True" && !results.Verify256Passed) algoResult.AddFailure("Verify256 failed");
				}
			}

			return algoResult;
		}
	}
}