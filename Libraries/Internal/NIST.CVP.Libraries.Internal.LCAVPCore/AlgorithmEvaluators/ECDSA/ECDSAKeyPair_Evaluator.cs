using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.ECDSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.ECDSA
{
	public class ECDSAKeyPair_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public ECDSAKeyPair_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new ECDSAKeyPair_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//CAVS allows for not checking a key generation method, but we now require it. So validate it.
				if (Options.GetValue("KeyPair_ExtraRandomBits") != "True" && Options.GetValue("KeyPair_TestingCandidates") != "True")
				{
					algoResult.AddFailure("No ECDSA KeyPair generation method was selected. CAVS allows this, but one must now be selected.");
				}

				if (Options.GetValue("KeyPair_P-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224:")?.Pass ?? false)) algoResult.AddFailure("KeyPair P-224 failed");
				if (Options.GetValue("KeyPair_P-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256:")?.Pass ?? false)) algoResult.AddFailure("KeyPair P-256 failed");
				if (Options.GetValue("KeyPair_P-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384:")?.Pass ?? false)) algoResult.AddFailure("KeyPair P-384 failed");
				if (Options.GetValue("KeyPair_P-521") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521:")?.Pass ?? false)) algoResult.AddFailure("KeyPair P-521 failed");
				if (Options.GetValue("KeyPair_K-233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233:")?.Pass ?? false)) algoResult.AddFailure("KeyPair K-233 failed");
				if (Options.GetValue("KeyPair_K-283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283:")?.Pass ?? false)) algoResult.AddFailure("KeyPair K-283 failed");
				if (Options.GetValue("KeyPair_K-409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409:")?.Pass ?? false)) algoResult.AddFailure("KeyPair K-409 failed");
				if (Options.GetValue("KeyPair_K-571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571:")?.Pass ?? false)) algoResult.AddFailure("KeyPair K-571 failed");
				if (Options.GetValue("KeyPair_B-233") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233:")?.Pass ?? false)) algoResult.AddFailure("KeyPair B-233 failed");
				if (Options.GetValue("KeyPair_B-283") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283:")?.Pass ?? false)) algoResult.AddFailure("KeyPair B-283 failed");
				if (Options.GetValue("KeyPair_B-409") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409:")?.Pass ?? false)) algoResult.AddFailure("KeyPair B-409 failed");
				if (Options.GetValue("KeyPair_B-571") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571:")?.Pass ?? false)) algoResult.AddFailure("KeyPair B-571 failed");
			}

			return algoResult;
		}
	}
}
