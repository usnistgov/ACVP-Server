using System.Collections.Generic;

namespace ACVPWorkflow.Results
{
	public class KillThisResult
	{
		public List<string> Errors { get; set; } = new List<string>();
		public bool IsSuccess { get => Errors.Count == 0; }

		public KillThisResult() { }
		public KillThisResult(string errorMessage)
		{
			Errors.Add(errorMessage);
		}
	}
}
