using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.ECDSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.ECDSA
{
	public class ECDSASigGen_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public ECDSASigGen_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new ECDSASigGen_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//if (Options.GetValue("SigGen_P-224_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-1 failed");
				if (Options.GetValue("SigGen_P-224_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-224 failed");
				if (Options.GetValue("SigGen_P-224_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-256 failed");
				if (Options.GetValue("SigGen_P-224_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-384 failed");
				if (Options.GetValue("SigGen_P-224_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-512 failed");
				if (Options.GetValue("SigGen_P-224_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-512224 failed");
				if (Options.GetValue("SigGen_P-224_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-224,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-224,SHA-512256 failed");
				//if (Options.GetValue("SigGen_P-256_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-1 failed");
				if (Options.GetValue("SigGen_P-256_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-224 failed");
				if (Options.GetValue("SigGen_P-256_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-256 failed");
				if (Options.GetValue("SigGen_P-256_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-384 failed");
				if (Options.GetValue("SigGen_P-256_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-512 failed");
				if (Options.GetValue("SigGen_P-256_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-512224 failed");
				if (Options.GetValue("SigGen_P-256_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-256,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-256,SHA-512256 failed");
				//if (Options.GetValue("SigGen_P-384_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-1 failed");
				if (Options.GetValue("SigGen_P-384_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-224 failed");
				if (Options.GetValue("SigGen_P-384_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-256 failed");
				if (Options.GetValue("SigGen_P-384_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-384 failed");
				if (Options.GetValue("SigGen_P-384_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-512 failed");
				if (Options.GetValue("SigGen_P-384_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-512224 failed");
				if (Options.GetValue("SigGen_P-384_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-384,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-384,SHA-512256 failed");
				//if (Options.GetValue("SigGen_P-521_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-1 failed");
				if (Options.GetValue("SigGen_P-521_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-224 failed");
				if (Options.GetValue("SigGen_P-521_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-256 failed");
				if (Options.GetValue("SigGen_P-521_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-384 failed");
				if (Options.GetValue("SigGen_P-521_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-512 failed");
				if (Options.GetValue("SigGen_P-521_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-512224 failed");
				if (Options.GetValue("SigGen_P-521_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "P-521,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen P-521,SHA-512256 failed");
				//if (Options.GetValue("SigGen_K-233_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-1 failed");
				if (Options.GetValue("SigGen_K-233_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-224 failed");
				if (Options.GetValue("SigGen_K-233_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-256 failed");
				if (Options.GetValue("SigGen_K-233_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-384 failed");
				if (Options.GetValue("SigGen_K-233_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-512 failed");
				if (Options.GetValue("SigGen_K-233_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-512224 failed");
				if (Options.GetValue("SigGen_K-233_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-233,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-233,SHA-512256 failed");
				//if (Options.GetValue("SigGen_K-283_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-1 failed");
				if (Options.GetValue("SigGen_K-283_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-224 failed");
				if (Options.GetValue("SigGen_K-283_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-256 failed");
				if (Options.GetValue("SigGen_K-283_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-384 failed");
				if (Options.GetValue("SigGen_K-283_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-512 failed");
				if (Options.GetValue("SigGen_K-283_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-512224 failed");
				if (Options.GetValue("SigGen_K-283_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-283,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-283,SHA-512256 failed");
				//if (Options.GetValue("SigGen_K-409_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-1 failed");
				if (Options.GetValue("SigGen_K-409_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-224 failed");
				if (Options.GetValue("SigGen_K-409_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-256 failed");
				if (Options.GetValue("SigGen_K-409_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-384 failed");
				if (Options.GetValue("SigGen_K-409_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-512 failed");
				if (Options.GetValue("SigGen_K-409_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-512224 failed");
				if (Options.GetValue("SigGen_K-409_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-409,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-409,SHA-512256 failed");
				//if (Options.GetValue("SigGen_K-571_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-1 failed");
				if (Options.GetValue("SigGen_K-571_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-224 failed");
				if (Options.GetValue("SigGen_K-571_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-256 failed");
				if (Options.GetValue("SigGen_K-571_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-384 failed");
				if (Options.GetValue("SigGen_K-571_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-512 failed");
				if (Options.GetValue("SigGen_K-571_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-512224 failed");
				if (Options.GetValue("SigGen_K-571_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "K-571,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen K-571,SHA-512256 failed");
				//if (Options.GetValue("SigGen_B-233_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-1 failed");
				if (Options.GetValue("SigGen_B-233_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-224 failed");
				if (Options.GetValue("SigGen_B-233_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-256 failed");
				if (Options.GetValue("SigGen_B-233_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-384 failed");
				if (Options.GetValue("SigGen_B-233_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-512 failed");
				if (Options.GetValue("SigGen_B-233_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-512224 failed");
				if (Options.GetValue("SigGen_B-233_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-233,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-233,SHA-512256 failed");
				//if (Options.GetValue("SigGen_B-283_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-1 failed");
				if (Options.GetValue("SigGen_B-283_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-224 failed");
				if (Options.GetValue("SigGen_B-283_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-256 failed");
				if (Options.GetValue("SigGen_B-283_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-384 failed");
				if (Options.GetValue("SigGen_B-283_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-512 failed");
				if (Options.GetValue("SigGen_B-283_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-512224 failed");
				if (Options.GetValue("SigGen_B-283_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-283,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-283,SHA-512256 failed");
				//if (Options.GetValue("SigGen_B-409_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-1 failed");
				if (Options.GetValue("SigGen_B-409_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-224 failed");
				if (Options.GetValue("SigGen_B-409_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-256 failed");
				if (Options.GetValue("SigGen_B-409_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-384 failed");
				if (Options.GetValue("SigGen_B-409_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-512 failed");
				if (Options.GetValue("SigGen_B-409_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-512224 failed");
				if (Options.GetValue("SigGen_B-409_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-409,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-409,SHA-512256 failed");
				//if (Options.GetValue("SigGen_B-571_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-1 failed");
				if (Options.GetValue("SigGen_B-571_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-224 failed");
				if (Options.GetValue("SigGen_B-571_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-256 failed");
				if (Options.GetValue("SigGen_B-571_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-384 failed");
				if (Options.GetValue("SigGen_B-571_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-512 failed");
				if (Options.GetValue("SigGen_B-571_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-512224:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-512224 failed");
				if (Options.GetValue("SigGen_B-571_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "B-571,SHA-512256:")?.Pass ?? false)) algoResult.AddFailure("SigGen B-571,SHA-512256 failed");
			}

			return algoResult;
		}
	}
}
