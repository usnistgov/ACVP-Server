using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.DSA;

namespace LCAVPCore.AlgorithmEvaluators.DSA
{
	public class DSASigVer_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public DSASigVer_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new DSASigVer_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//SigVer
				if (Options.GetValue("SigVer_L1024N160_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-1 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-224 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-256 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-384 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-512 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-512224 failed");
				if (Options.GetValue("SigVer_L1024N160_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=1024, N=160, SHA-512256 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-1 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-224 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-256 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-384 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-512 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-512224 failed");
				if (Options.GetValue("SigVer_L2048N224_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=224, SHA-512256 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-1 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-224 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-256 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-384 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-512 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-512224 failed");
				if (Options.GetValue("SigVer_L2048N256_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=2048, N=256, SHA-512256 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-1") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-1 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-224 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-256 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-384") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-384 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-512") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-512 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-512224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-512224 failed");
				if (Options.GetValue("SigVer_L3072N256_SHA-512256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("SigVer Mod L=3072, N=256, SHA-512256 failed");
			}

			return algoResult;
		}
	}
}
