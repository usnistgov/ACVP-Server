using System.Collections.Generic;
using LCAVPCore.AlgorithmResultsExtractors.SHA_3;

namespace LCAVPCore.AlgorithmEvaluators.SHA_3
{
	public class SHAKE_256_Evaluator : AlgorithmEvaluatorBase, IAlgorithmEvaluator
	{
		public SHAKE_256_Evaluator(Dictionary<string, string> options, string submissionPath) : base(options, submissionPath)
		{
		}

		public AlgorithmEvaluationResult Evaluate()
		{
			var resultExtractor = new SHAKE_256_ResultsExtractor(SubmissionPath);
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

				//Byte-oriented must match - input and output
				if (bool.Parse(Options.GetValue("SHAKE256_Byte")) != results.InputByteOrientedOnly) algoResult.AddFailure("InputByteOrientedOnly does not match");
				if (bool.Parse(Options.GetValue("SHAKE256_OutputByteOnly")) != results.OutputByteOrientedOnly) algoResult.AddFailure("OutputByteOrientedOnly does not match");

				//If SHA_NoNull = true, then the message must be there, otherwise don't care?
				if (bool.Parse(Options.GetValue("SHAKE_NoNull")) != results.DoesNotSupportNullMessage) algoResult.AddFailure("DoesNotSupportNullMessage does not match");
			}

			return algoResult;
		}
	}
}
