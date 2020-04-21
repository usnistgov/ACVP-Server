using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.DSA
{
	public class DSAKeyPair_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public DSAKeyPair_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new DSAKeyPair_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("KeyPair_L2048N224") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=224:")?.Pass ?? false)) algoResult.AddFailure("KeyPair Mod L=2048, N=224 failed");
				if (Options.GetValue("KeyPair_L2048N256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=2048, N=256:")?.Pass ?? false)) algoResult.AddFailure("KeyPair Mod L=2048, N=256 failed");
				if (Options.GetValue("KeyPair_L3072N256") == "True" && !(results.Results.FirstOrDefault(r => r.TestName == "Mod L=3072, N=256:")?.Pass ?? false)) algoResult.AddFailure("KeyPair Mod L=3072, N=256 failed");
			}
			return algoResult;
		}
	}
}
