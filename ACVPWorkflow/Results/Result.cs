using System.Collections.Generic;

namespace ACVPWorkflow.Results
{
	public class Result
	{
		public List<string> Errors { get; set; } = new List<string>();
		public bool IsSuccess { get => Errors.Count == 0; }

		public Result() { }
		public Result(string errorMessage)
		{
			Errors.Add(errorMessage);
		}
	}
}
