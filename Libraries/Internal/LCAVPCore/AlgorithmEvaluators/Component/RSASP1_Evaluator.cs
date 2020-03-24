using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class RSASP1_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSASP1_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSASP1_ResultsExtractor(SubmissionPath);
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
