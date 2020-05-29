using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface ISubmissionProcessor
	{
		List<ProcessingResult> Process(string submissionRoot);
	}
}
