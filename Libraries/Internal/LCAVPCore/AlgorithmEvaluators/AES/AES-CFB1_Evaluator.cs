using System.Collections.Generic;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_CFB1_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_CFB1_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_CFB1_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Verify the appropriate tests based on the CFB1_128 state
				switch (Options.GetValue("CFB1_128_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt128, AlgorithmTestNames.AES_CFB1, "Decrypt 128"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt128, AlgorithmTestNames.AES_CFB1, "Encrypt 128"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt128, AlgorithmTestNames.AES_CFB1, "Decrypt 128"));
						algoResult.AddFailure(CheckTests(results.Encrypt128, AlgorithmTestNames.AES_CFB1, "Encrypt 128"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid CFB1_128 State");
						break;
				}


				//Verify the appropriate tests based on the CFB1_192 state
				switch (Options.GetValue("CFB1_192_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt192, AlgorithmTestNames.AES_CFB1, "Decrypt 192"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt192, AlgorithmTestNames.AES_CFB1, "Encrypt 192"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt192, AlgorithmTestNames.AES_CFB1, "Decrypt 192"));
						algoResult.AddFailure(CheckTests(results.Encrypt192, AlgorithmTestNames.AES_CFB1, "Encrypt 192"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid CFB1_192 State");
						break;
				}

				//Verify the appropriate tests based on the CFB1_256 state
				switch (Options.GetValue("CFB1_256_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt256, AlgorithmTestNames.AES_CFB1, "Decrypt 256"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt256, AlgorithmTestNames.AES_CFB1, "Encrypt 256"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt256, AlgorithmTestNames.AES_CFB1, "Decrypt 256"));
						algoResult.AddFailure(CheckTests(results.Encrypt256, AlgorithmTestNames.AES_CFB1, "Encrypt 256"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid CFB1_256 State");
						break;
				}
			}

			return algoResult;
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