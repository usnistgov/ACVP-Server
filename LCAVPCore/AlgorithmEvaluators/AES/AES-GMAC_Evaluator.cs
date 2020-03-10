using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_GMAC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_GMAC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_GMAC_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("Encrypt") == "True")
				{
					if (Options.GetValue("Keysize_128") == "True" && !results.Encrypt128Passed) algoResult.AddFailure("Encrypt128 failed");
					if (Options.GetValue("Keysize_192") == "True" && !results.Encrypt192Passed) algoResult.AddFailure("Encrypt192 failed");
					if (Options.GetValue("Keysize_256") == "True" && !results.Encrypt256Passed) algoResult.AddFailure("Encrypt256 failed");
				}

				if (Options.GetValue("Decrypt") == "True")
				{
					if (Options.GetValue("Keysize_128") == "True" && !results.Decrypt128Passed) algoResult.AddFailure("Decrypt128 failed");
					if (Options.GetValue("Keysize_192") == "True" && !results.Decrypt192Passed) algoResult.AddFailure("Decrypt192 failed");
					if (Options.GetValue("Keysize_256") == "True" && !results.Decrypt256Passed) algoResult.AddFailure("Decrypt256 failed");
				}
			}

			return algoResult;
		}
	}
}