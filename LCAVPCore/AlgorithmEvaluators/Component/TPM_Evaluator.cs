using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class TPM_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public TPM_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new TPM_ResultsExtractor(SubmissionPath);
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
