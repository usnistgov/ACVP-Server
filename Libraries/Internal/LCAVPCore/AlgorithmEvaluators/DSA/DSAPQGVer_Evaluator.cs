using System.Collections.Generic;
using System.Linq;
using LCAVPCore.AlgorithmResultsExtractors.DSA;

namespace LCAVPCore.AlgorithmEvaluators.DSA
{
	public class DSAPQGVer_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public DSAPQGVer_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new DSAPQGVer_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//PQGVer Probable Prime
				if (Options.GetValue("PQGVer_ProbablePrimePQ") == "True")
				{
					if (Options.GetValue("PQGVer_L1024N160_SHA-1") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-1 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-224") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-256 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-384") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-384 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-512 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512224") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=1024, N=160, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-224") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-384") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512224") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-384") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-384") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512256") == "True" && !(results.PQGVer_ProbablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProbablePrime Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGVer Provable Prime
				if (Options.GetValue("PQGVer_ProvablePrimePQ") == "True")
				{
					if (Options.GetValue("PQGVer_L1024N160_SHA-1") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-1 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-224") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-256 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-384") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-384 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-512 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512224") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=1024, N=160, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-224") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-384") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512224") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-384") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-384") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512256") == "True" && !(results.PQGVer_ProvablePrime.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer ProvablePrime Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGVer Unverifiable G
				if (Options.GetValue("PQGVer_UnverifiableG") == "True")
				{
					if (Options.GetValue("PQGVer_L1024N160_SHA-1") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-1 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-224") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-256 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-384") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-384 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-512 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512224") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=1024, N=160, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-224") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-384") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512224") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-384") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-384") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512256") == "True" && !(results.PQGVer_UnverifiableG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer UnverifiableG Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGVer Canonical G
				if (Options.GetValue("PQGVer_CanonicalG") == "True")
				{
					if (Options.GetValue("PQGVer_L1024N160_SHA-1") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-1:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-1 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-224") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-256 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-384") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-384 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-512 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512224") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L1024N160_SHA-512256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=1024, N=160, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=1024, N=160, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-224") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-384") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512224") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/224:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-512224 failed");
					if (Options.GetValue("PQGVer_L2048N224_SHA-512256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=224, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-384") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L2048N256_SHA-512256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=2048, N=256, SHA-512256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=3072, N=256, SHA-256 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-384") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-384:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=3072, N=256, SHA-384 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=3072, N=256, SHA-512 failed");
					if (Options.GetValue("PQGVer_L3072N256_SHA-512256") == "True" && !(results.PQGVer_CanonicalG.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256, SHA-512/256:")?.Pass ?? false)) algoResult.AddFailure("PQGVer CanonicalG Mod L=3072, N=256, SHA-512256 failed");
				}

				//PQGVer 186-2
				if (Options.GetValue("PQGVer_FIPS186-2PQGVerTest") == "True" && !(results.PQGVer_186_2.FirstOrDefault(r => r.TestName == "Mod 1024:")?.Pass ?? false)) algoResult.AddFailure("186-2 PQGVer Mod 1024 failed");

			}

			return algoResult;
		}
	}
}
