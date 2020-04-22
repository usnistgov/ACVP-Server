using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.TDES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.TDES
{
	public class TDES_CTR_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public TDES_CTR_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new TDES_CTR_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Just double check that they actually checked Encrypt, and then that the single test passed
				if (Options.GetValue("CTR_State") == "Encrypt" && !results.ForwardCipherFunctionTestPassed) algoResult.AddFailure("ForwardCipherFunctionTest failed");
			}

			return algoResult;
		}
	}
}