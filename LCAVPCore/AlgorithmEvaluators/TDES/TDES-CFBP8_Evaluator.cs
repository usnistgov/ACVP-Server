using System.Collections.Generic;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResultsExtractors.TDES;

namespace LCAVPCore.AlgorithmEvaluators.TDES
{
	public class TDES_CFBP8_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public TDES_CFBP8_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new TDES_CFBP8_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Keying option - 1 if says so, otherwise must be 2
				int keyingOption = Options.GetValue("CFBP_KEY_CHOICE1") == "Yes" ? 1 : 2;

			//Verify the appropriate tests based on the state
			switch (Options.GetValue("CFBP8_State"))
			{
				case "Decrypt":
					algoResult.AddFailure(CheckTests(results.Decrypt, DecryptTestNamesForKeyingOption(keyingOption), $"Decrypt KO {keyingOption.ToString()}"));
					break;

				case "Encrypt":
					algoResult.AddFailure(CheckTests(results.Encrypt, AlgorithmTestNames.TDES_Encrypt, "Encrypt"));
					break;

				case "Both":
					algoResult.AddFailure(CheckTests(results.Decrypt, DecryptTestNamesForKeyingOption(keyingOption), $"Decrypt KO {keyingOption.ToString()}"));
					algoResult.AddFailure(CheckTests(results.Encrypt, AlgorithmTestNames.TDES_Encrypt, "Encrypt"));
					break;

				case "False":
					break;

				default:
						algoResult.AddFailure("Invalid CFBP8 State");
						break;
				}
			}

			return algoResult;
		}

		private string[] DecryptTestNamesForKeyingOption(int keyingOption)
		{
			switch (keyingOption)
			{
				case 1: return AlgorithmTestNames.TDES_Decrypt_KO1;
				case 2: return AlgorithmTestNames.TDES_Decrypt_KO2;
				default: return new string[] { "kjadslkjflakuhaliuha" };    //Nonsense test name, so it has to fail
			}
		}

		private List<string> CheckTests(List<PassFailResult> results, string[] tests, string testNamePrefix)
		{
			List<string> failures = new List<string>();

			if (results == null || results.Count == 0)
			{
				failures.Add("Missing results");
				return failures;
			}

			foreach (string testName in tests)
			{
				if (!results.Exists(r => r.TestName == testName && r.Pass))
				{
					failures.Add($"{testNamePrefix} {testName} failed");
				}
			}

			return failures;
		}
	}
}