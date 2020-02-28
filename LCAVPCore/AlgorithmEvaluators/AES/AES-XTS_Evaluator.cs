using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_XTS_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_XTS_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_XTS_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("XTS_AES128_Encrypt") == "True" && !results.Encrypt128Passed) algoResult.AddFailure("Encrypt128 failed");
				if (Options.GetValue("XTS_AES128_Decrypt") == "True" && !results.Decrypt128Passed) algoResult.AddFailure("Decrypt128 failed");
				if (Options.GetValue("XTS_AES256_Encrypt") == "True" && !results.Encrypt256Passed) algoResult.AddFailure("Encrypt256 failed");
				if (Options.GetValue("XTS_AES256_Decrypt") == "True" && !results.Decrypt256Passed) algoResult.AddFailure("Decrypt256 failed");
			}

			return algoResult;
		}
	}
}