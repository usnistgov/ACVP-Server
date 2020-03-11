using System.Collections.Generic;

namespace LCAVPCore
{
	public class ProcessingResult
	{
		public bool Success
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public ProcessingType Type { get; set; }
		public WorkflowType? WorkflowType { get; set; }

		public string RegistrationJson { get; set; }

		public object TheThingy { get; set; }

		public List<string> Errors { get; set; } = new List<string>();

		public ProcessingResult() { }
	}

	public enum ProcessingType
	{
		New,
		Change,
		Update
	}
}
