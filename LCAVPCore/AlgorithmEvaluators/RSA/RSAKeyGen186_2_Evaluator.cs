using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.RSA;

namespace LCAVPCore.AlgorithmEvaluators.RSA
{
	public class RSAKeyGen186_2_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public RSAKeyGen186_2_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new RSAKeyGen186_2_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod1024") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True" && !TestPassed(results.Results, "Mod 1024 Key     3")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1024 Key 3 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True" && !TestPassed(results.Results, "Mod 1024 Key    17")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1024 Key 17 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True" && !TestPassed(results.Results, "Mod 1024 Key 65537")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1024 Key 65537 failed");
				}

				if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod1536") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True" && !TestPassed(results.Results, "Mod 1536 Key     3")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1536 Key 3 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True" && !TestPassed(results.Results, "Mod 1536 Key    17")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1536 Key 17 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True" && !TestPassed(results.Results, "Mod 1536 Key 65537")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 1536 Key 65537 failed");
				}

				if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod2048") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True" && !TestPassed(results.Results, "Mod 2048 Key     3")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 2048 Key 3 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True" && !TestPassed(results.Results, "Mod 2048 Key    17")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 2048 Key 17 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True" && !TestPassed(results.Results, "Mod 2048 Key 65537")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 2048 Key 65537 failed");
				}

				if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod3072") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True" && !TestPassed(results.Results, "Mod 3072 Key     3")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 3072 Key 3 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True" && !TestPassed(results.Results, "Mod 3072 Key    17")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 3072 Key 17 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True" && !TestPassed(results.Results, "Mod 3072 Key 65537")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 3072 Key 65537 failed");
				}

				if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod4096") == "True")
				{
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True" && !TestPassed(results.Results, "Mod 4096 Key     3")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 4096 Key 3 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True" && !TestPassed(results.Results, "Mod 4096 Key    17")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 4096 Key 17 failed");
					if (Options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True" && !TestPassed(results.Results, "Mod 4096 Key 65537")) algoResult.AddFailure("FIPS 186-2 KeyGen Mod 4096 Key 65537 failed");
				}
			}

			return algoResult;
		}
	}
}