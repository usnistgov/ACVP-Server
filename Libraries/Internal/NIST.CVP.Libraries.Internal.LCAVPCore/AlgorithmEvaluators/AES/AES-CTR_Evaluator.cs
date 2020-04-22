using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_CTR_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_CTR_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_CTR_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("CTR128_State") == "Encrypt" && !results.Passed128) algoResult.AddFailure("128 failed");
				if (Options.GetValue("CTR192_State") == "Encrypt" && !results.Passed192) algoResult.AddFailure("192 failed");
				if (Options.GetValue("CTR256_State") == "Encrypt" && !results.Passed256) algoResult.AddFailure("256 failed");
			}

			return algoResult;
		}
	}
}