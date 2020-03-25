using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.AES;

namespace LCAVPCore.AlgorithmEvaluators.AES
{
	public class AES_KWP_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public AES_KWP_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new AES_KWP_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Test enabling is nested - need action (encrypt/decrypt), key length (128/192/256), and cipher direction (FWD/INV)
				if (Options.GetValue("KWP_AE") == "True")
				{
					if (Options.GetValue("KWP_FWD_CIPHER") == "True")
					{
						if (Options.GetValue("KWP_AES_128") == "True" && !results.Encrypt128Passed) algoResult.AddFailure("Encrypt128 failed");
						if (Options.GetValue("KWP_AES_192") == "True" && !results.Encrypt192Passed) algoResult.AddFailure("Encrypt192 failed");
						if (Options.GetValue("KWP_AES_256") == "True" && !results.Encrypt256Passed) algoResult.AddFailure("Encrypt256 failed");
					}

					if (Options.GetValue("KWP_INV_CIPHER") == "True")
					{
						if (Options.GetValue("KWP_AES_128") == "True" && !results.Encrypt128InversePassed) algoResult.AddFailure("Encrypt128Inverse failed");
						if (Options.GetValue("KWP_AES_192") == "True" && !results.Encrypt192InversePassed) algoResult.AddFailure("Encrypt192Inverse failed");
						if (Options.GetValue("KWP_AES_256") == "True" && !results.Encrypt256InversePassed) algoResult.AddFailure("Encrypt256Inverse failed");
					}
				}

				if (Options.GetValue("KWP_AD") == "True")
				{
					if (Options.GetValue("KWP_FWD_CIPHER") == "True")
					{
						if (Options.GetValue("KWP_AES_128") == "True" && !results.Decrypt128Passed) algoResult.AddFailure("Decrypt128 failed");
						if (Options.GetValue("KWP_AES_192") == "True" && !results.Decrypt192Passed) algoResult.AddFailure("Decrypt192 failed");
						if (Options.GetValue("KWP_AES_256") == "True" && !results.Decrypt256Passed) algoResult.AddFailure("Decrypt256 failed");
					}

					if (Options.GetValue("KWP_INV_CIPHER") == "True")
					{
						if (Options.GetValue("KWP_AES_128") == "True" && !results.Decrypt128InversePassed) algoResult.AddFailure("Decrypt128Inverse failed");
						if (Options.GetValue("KWP_AES_192") == "True" && !results.Decrypt192InversePassed) algoResult.AddFailure("Decrypt192Inverse failed");
						if (Options.GetValue("KWP_AES_256") == "True" && !results.Decrypt256InversePassed) algoResult.AddFailure("Decrypt256Inverse failed");
					}
				}
			}

			return algoResult;
		}
	}
}