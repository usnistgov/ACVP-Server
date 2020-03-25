using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.Component;

namespace LCAVPCore.AlgorithmEvaluators.Component
{
	public class ECC_CDH_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public ECC_CDH_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new ECC_CDH_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//For each option, see if checked, then get the corresponding result - or default to false (failed) if the test is not found
				if (Options.GetValue("KASECC_Comp_DLC_Prim_P224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[P-224]")?.Pass ?? false)) algoResult.AddFailure("P-224 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_P256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[P-256]")?.Pass ?? false)) algoResult.AddFailure("P-256 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_P384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[P-384]")?.Pass ?? false)) algoResult.AddFailure("P-384 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_P521") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[P-521]")?.Pass ?? false)) algoResult.AddFailure("P-521 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_K233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[K-233]")?.Pass ?? false)) algoResult.AddFailure("K-233 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_K283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[K-283]")?.Pass ?? false)) algoResult.AddFailure("K-283 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_K409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[K-409]")?.Pass ?? false)) algoResult.AddFailure("K-409 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_K571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[K-571]")?.Pass ?? false)) algoResult.AddFailure("K-571 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_B233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[B-233]")?.Pass ?? false)) algoResult.AddFailure("B-233 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_B283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[B-283]")?.Pass ?? false)) algoResult.AddFailure("B-283 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_B409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[B-409]")?.Pass ?? false)) algoResult.AddFailure("B-409 failed");
				if (Options.GetValue("KASECC_Comp_DLC_Prim_B571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "[B-571]")?.Pass ?? false)) algoResult.AddFailure("B-571 failed");
			}

			return algoResult;
		}
	}
}
