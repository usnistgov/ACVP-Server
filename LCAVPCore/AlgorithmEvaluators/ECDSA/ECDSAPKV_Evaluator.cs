using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.ECDSA;

namespace LCAVPCore.AlgorithmEvaluators.ECDSA
{
	public class ECDSAPKV_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public ECDSAPKV_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new ECDSAPKV_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("PKV_P-192") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-192:")?.Pass ?? false)) algoResult.AddFailure("PKV P-192 failed");
				if (Options.GetValue("PKV_P-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224:")?.Pass ?? false)) algoResult.AddFailure("PKV P-224 failed");
				if (Options.GetValue("PKV_P-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256:")?.Pass ?? false)) algoResult.AddFailure("PKV P-256 failed");
				if (Options.GetValue("PKV_P-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384:")?.Pass ?? false)) algoResult.AddFailure("PKV P-384 failed");
				if (Options.GetValue("PKV_P-521") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521:")?.Pass ?? false)) algoResult.AddFailure("PKV P-521 failed");
				if (Options.GetValue("PKV_K-163") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-163:")?.Pass ?? false)) algoResult.AddFailure("PKV K-163 failed");
				if (Options.GetValue("PKV_K-233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233:")?.Pass ?? false)) algoResult.AddFailure("PKV K-233 failed");
				if (Options.GetValue("PKV_K-283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283:")?.Pass ?? false)) algoResult.AddFailure("PKV K-283 failed");
				if (Options.GetValue("PKV_K-409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409:")?.Pass ?? false)) algoResult.AddFailure("PKV K-409 failed");
				if (Options.GetValue("PKV_K-571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571:")?.Pass ?? false)) algoResult.AddFailure("PKV K-571 failed");
				if (Options.GetValue("PKV_B-163") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-163:")?.Pass ?? false)) algoResult.AddFailure("PKV B-163 failed");
				if (Options.GetValue("PKV_B-233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233:")?.Pass ?? false)) algoResult.AddFailure("PKV B-233 failed");
				if (Options.GetValue("PKV_B-283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283:")?.Pass ?? false)) algoResult.AddFailure("PKV B-283 failed");
				if (Options.GetValue("PKV_B-409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409:")?.Pass ?? false)) algoResult.AddFailure("PKV B-409 failed");
				if (Options.GetValue("PKV_B-571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571:")?.Pass ?? false)) algoResult.AddFailure("PKV B-571 failed");
			}

			return algoResult;
		}
	}
}
