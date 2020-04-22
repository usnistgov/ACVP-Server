using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.SHS;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators.SHS
{
	public class SHA_512256_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public SHA_512256_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new SHA_512256_ResultsExtractor(SubmissionPath);
			var results = resultExtractor.Extract();

			AlgorithmEvaluationResult algoResult = new AlgorithmEvaluationResult();

			//Fail if invalid results were found during parse
			if (!results.Valid)
			{
				algoResult.AddFailure(results.InvalidReasons);
			}
			else
			{
				//Go through the algo's options, figure out what needs to be tested, and check for it
				//First, the basic did this thing pass check (since there's only 1 result)
				if (!results.Pass) algoResult.AddFailure("Test failed");

				//Byte-oriented must match
				if (bool.Parse(Options.GetValue("SHA512_256_Byte")) != results.ByteOrientedOnly) algoResult.AddFailure("ByteOrientedOnly does not match");

				//If SHA_NoNull = true, then the message must be there, otherwise don't care?
				if (bool.Parse(Options.GetValue("SHA_NoNull")) != results.DoesNotSupportNullMessage) algoResult.AddFailure("DoesNotSupportNullMessage does not match");
			}

			return algoResult;
		}
	}
}