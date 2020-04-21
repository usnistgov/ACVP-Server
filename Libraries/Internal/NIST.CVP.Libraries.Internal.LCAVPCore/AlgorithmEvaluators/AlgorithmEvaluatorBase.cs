using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators
{
	public class AlgorithmEvaluatorBase
	{
		protected Dictionary<string, string> Options { get; private set; }
		protected string SubmissionPath { get; private set; }

		protected AlgorithmEvaluatorBase(Dictionary<string, string> options, string submissionPath)
		{
			Options = options;
			SubmissionPath = submissionPath;
		}

		protected bool TestPassed(List<PassFailResult> results, string testName)
		{
			if (results == null) return false;

			return results.Exists(r => r.TestName == testName && r.Pass);
		}
	}
}