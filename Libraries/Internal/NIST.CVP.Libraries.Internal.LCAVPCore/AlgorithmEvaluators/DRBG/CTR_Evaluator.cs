using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DRBG;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.DRBG
{
	public class CTR_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public CTR_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			//Get the result extractor - should use the factory
			var resultExtractor = new CTR_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{

				bool useDF = Options.GetValue("CTR_DRBG Use Df") == "True";
				bool noDF = Options.GetValue("CTR_DRBG No Df") == "True";

				if (Options.GetValue("CTR_DRBG 3KeyTDEA") == "True")
				{
					if (useDF && !TestPassed(results.Results, "3KeyTDEA use df:")) algoResult.AddFailure("3KeyTDEA use df failed");
					if (noDF && !TestPassed(results.Results, "3KeyTDEA no df:")) algoResult.AddFailure("3KeyTDEA no df failed");
				}

				if (Options.GetValue("CTR_DRBG AES-128") == "True")
				{
					if (useDF && !TestPassed(results.Results, "AES-128 use df:")) algoResult.AddFailure("AES-128 use df failed");
					if (noDF && !TestPassed(results.Results, "AES-128 no df:")) algoResult.AddFailure("AES-128 no df failed");
				}

				if (Options.GetValue("CTR_DRBG AES-192") == "True")
				{
					if (useDF && !TestPassed(results.Results, "AES-192 use df:")) algoResult.AddFailure("AES-192 use df failed");
					if (noDF && !TestPassed(results.Results, "AES-192 no df:")) algoResult.AddFailure("AES-192 no df failed");
				}

				if (Options.GetValue("CTR_DRBG AES-256") == "True")
				{
					if (useDF && !TestPassed(results.Results, "AES-256 use df:")) algoResult.AddFailure("AES-256 use df failed");
					if (noDF && !TestPassed(results.Results, "AES-256 no df:")) algoResult.AddFailure("AES-256 no df failed");
				}
			}

			return algoResult;
		}
	}
}
