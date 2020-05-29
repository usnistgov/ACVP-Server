using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.HMAC;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.HMAC
{
	public class HMAC_SHA2_512_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public HMAC_SHA2_512_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new HMAC_SHA2_512_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (!results.Passed) algoResult.AddFailure("Test failed");
			}

			return algoResult;
		}
	}
}
