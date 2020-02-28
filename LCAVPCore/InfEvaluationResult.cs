using System.Collections.Generic;

namespace LCAVPCore
{
	public class InfEvaluationResult
	{
		public List<AlgorithmEvaluationResult> AlgorithmResults { get; set; } = new List<AlgorithmEvaluationResult>();
		public List<string> Errors { get; set; } = new List<string>();
		public InfFile InfFile { get; set; }
		public string SpecialInstructions { get; set; }

		public bool IsValidFile
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public bool Success
		{
			get
			{
				//Success if no failed algorithm results
				return !AlgorithmResults.Exists(r => r.Fail);
			}
		}
	}
}
