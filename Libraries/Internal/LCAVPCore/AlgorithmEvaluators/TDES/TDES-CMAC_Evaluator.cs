using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.TDES;

namespace LCAVPCore.AlgorithmEvaluators.TDES
{
	public class TDES_CMAC_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public TDES_CMAC_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new TDES_CMAC_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("CMACGen_TDES2") == "True" && !results.KeySize2GeneratePassed) algoResult.AddFailure("CMACGen_TDES2 failed");
				if (Options.GetValue("CMACVer_TDES2") == "True" && !results.KeySize2VerifyPassed) algoResult.AddFailure("CMACVer_TDES2 failed");
				if (Options.GetValue("CMACGen_TDES3") == "True" && !results.KeySize3GeneratePassed) algoResult.AddFailure("CMACGen_TDES3 failed");
				if (Options.GetValue("CMACVer_TDES3") == "True" && !results.KeySize3VerifyPassed) algoResult.AddFailure("CMACVer_TDES3 failed");
			}

			return algoResult;
		}
	}
}