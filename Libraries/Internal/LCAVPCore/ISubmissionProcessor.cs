using System.Collections.Generic;

namespace LCAVPCore
{
	public interface ISubmissionProcessor
	{
		List<ProcessingResult> Process(string submissionRoot);
	}
}
