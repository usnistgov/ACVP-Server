using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_CCM_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_CCM_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_CCM_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("CCM_KeySize128") == "True" && !results.Passed128) algoResult.AddFailure("128 failed");
				if (Options.GetValue("CCM_KeySize192") == "True" && !results.Passed192) algoResult.AddFailure("192 failed");
				if (Options.GetValue("CCM_KeySize256") == "True" && !results.Passed256) algoResult.AddFailure("256 failed");
			}

			return algoResult;
		}
	}
}