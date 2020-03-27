using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class ANS_963_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public ANS_963_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new ANS_963_ResultsExtractor(SubmissionPath);
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

			int minKeyDataLen = ParsingHelper.ParseValueToInteger(Options.GetValue("KDF_800_135_ANSX963_2001_keydata_length1"));
			int maxKeyDataLen = ParsingHelper.ParseValueToInteger(Options.GetValue("KDF_800_135_ANSX963_2001_keydata_length2"));

			if (minKeyDataLen < 112 || maxKeyDataLen < 112 || minKeyDataLen > 4096 || maxKeyDataLen > 4096)
			{
				algoResult.AddFailure("Key Data Length must be between 112 and 4096. CAVS allowed values as small as 8, but they are no longer legal");
			}

			return algoResult;
		}
	}
}
