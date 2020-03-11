using System.Collections.Generic;

namespace LCAVPCore
{
	public class SubmissionProcessingResult
	{
		public bool Success
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public List<string> Errors { get; set; } = new List<string>();

		public List<ProcessingResult> ProcessingResults { get; set; } = new List<ProcessingResult>();

		public int SubmissionID { get; set; }
	}
}
