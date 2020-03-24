using System.Collections.Generic;

namespace LCAVPCore
{
	public class AlgorithmEvaluationResult
	{
		public List<string> FailureMessages { get; set; } = new List<string>();

		public bool Pass
		{
			get
			{
				return FailureMessages.Count == 0;
			}
		}

		public bool Fail { get { return !Pass; } }

		//These 2 methods enable laziness - shortcuts to avoid typing as much
		public void AddFailure(string failure)
		{
			if (!string.IsNullOrWhiteSpace(failure)) FailureMessages.Add(failure);
		}

		public void AddFailure(List<string> failures)
		{
			if (failures != null) FailureMessages.AddRange(failures);
		}
	}
}
