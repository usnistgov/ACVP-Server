using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.DSA;

namespace LCAVPCore.AlgorithmEvaluators.DSA
{
	public class DSAPQGGen_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public DSAPQGGen_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new DSAPQGGen_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//PQGGen Probable Prime
				if (Options.GetValue("PQGGen_ProbablePrimePQ") == "True")
				{
					if (Options.GetValue("PQGGen_L2048N224_SHA-224") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-384") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512224") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-384") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-384") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512256") == "True" && !(results.PQGGen_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProbablePrime Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGGen Provable Prime
				if (Options.GetValue("PQGGen_ProvablePrimePQ") == "True")
				{
					if (Options.GetValue("PQGGen_L2048N224_SHA-224") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-384") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512224") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-384") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-384") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512256") == "True" && !(results.PQGGen_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen ProvablePrime Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGGen Unverifiable G
				if (Options.GetValue("PQGGen_UnverifiableG") == "True")
				{
					if (Options.GetValue("PQGGen_L2048N224_SHA-224") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-384") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512224") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-384") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-384") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512256") == "True" && !(results.PQGGen_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen UnverifiableG Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGGen Canonical G
				if (Options.GetValue("PQGGen_CanonicalG") == "True")
				{
					if (Options.GetValue("PQGGen_L2048N224_SHA-224") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-384") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512224") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGGen_L2048N224_SHA-512256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-384") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L2048N256_SHA-512256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-384") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGGen_L3072N256_SHA-512256") == "True" && !(results.PQGGen_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGGen CanonicalG Mod L=3072, N=256, SHA-512256 failed");
				}
			}

			return algoResult;
		}
	}
}
