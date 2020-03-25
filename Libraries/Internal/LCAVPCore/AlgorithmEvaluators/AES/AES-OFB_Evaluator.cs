using System.Collections.Generic;
using LCAVPCore.AlgorithmResults;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_OFB_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_OFB_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_OFB_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Verify the appropriate tests based on the OFB128 state
				switch (Options.GetValue("OFB128_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt128, AlgorithmTestNames.AES_OFB, "Decrypt 128"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt128, AlgorithmTestNames.AES_OFB, "Encrypt 128"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt128, AlgorithmTestNames.AES_OFB, "Decrypt 128"));
						algoResult.AddFailure(CheckTests(results.Encrypt128, AlgorithmTestNames.AES_OFB, "Encrypt 128"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid OFB128 State");
						break;
				}


				//Verify the appropriate tests based on the OFB192 state
				switch (Options.GetValue("OFB192_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt192, AlgorithmTestNames.AES_OFB, "Decrypt 192"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt192, AlgorithmTestNames.AES_OFB, "Encrypt 192"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt192, AlgorithmTestNames.AES_OFB, "Decrypt 192"));
						algoResult.AddFailure(CheckTests(results.Encrypt192, AlgorithmTestNames.AES_OFB, "Encrypt 192"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid OFB192 State");
						break;
				}

				//Verify the appropriate tests based on the OFB256 state
				switch (Options.GetValue("OFB256_State"))
				{
					case "Decrypt":
						algoResult.AddFailure(CheckTests(results.Decrypt256, AlgorithmTestNames.AES_OFB, "Decrypt 256"));
						break;

					case "Encrypt":
						algoResult.AddFailure(CheckTests(results.Encrypt256, AlgorithmTestNames.AES_OFB, "Encrypt 256"));
						break;

					case "Both":
						algoResult.AddFailure(CheckTests(results.Decrypt256, AlgorithmTestNames.AES_OFB, "Decrypt 256"));
						algoResult.AddFailure(CheckTests(results.Encrypt256, AlgorithmTestNames.AES_OFB, "Encrypt 256"));
						break;

					case "False":
						break;

					default:
						algoResult.AddFailure("Invalid OFB256 State");
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