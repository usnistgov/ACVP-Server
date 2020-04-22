using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DRBG;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.DRBG
{
	public class Hash_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public Hash_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new Hash_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				if (Options.GetValue("Hash_DRBG SHA-1") == "True" && !TestPassed(results.Results, "SHA-1:")) algoResult.AddFailure("SHA-1 failed");
				if (Options.GetValue("Hash_DRBG SHA-224") == "True" && !TestPassed(results.Results, "SHA-224:")) algoResult.AddFailure("SHA-224 failed");
				if (Options.GetValue("Hash_DRBG SHA-256") == "True" && !TestPassed(results.Results, "SHA-256:")) algoResult.AddFailure("SHA-256 failed");
				if (Options.GetValue("Hash_DRBG SHA-384") == "True" && !TestPassed(results.Results, "SHA-384:")) algoResult.AddFailure("SHA-384 failed");
				if (Options.GetValue("Hash_DRBG SHA-512") == "True" && !TestPassed(results.Results, "SHA-512:")) algoResult.AddFailure("SHA-512 failed");
				if (Options.GetValue("Hash_DRBG SHA-512_224") == "True" && !TestPassed(results.Results, "SHA-512/224:")) algoResult.AddFailure("SHA-512/224 failed");
				if (Options.GetValue("Hash_DRBG SHA-512_256") == "True" && !TestPassed(results.Results, "SHA-512/256:")) algoResult.AddFailure("SHA-512/256 failed");
			}

			//Finally return whether this algorithm passes
			return algoResult;
		}
	}
}
