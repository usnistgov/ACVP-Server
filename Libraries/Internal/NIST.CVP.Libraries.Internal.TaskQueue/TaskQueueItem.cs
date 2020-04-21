using System;

namespace NIST.CVP.Libraries.Internal.TaskQueue
{
	public class TaskQueueItem
	{
		public long ID { get; set; }
		public TaskType Type { get; set; }

		public string TaskTypeText
		{
			get => Type switch
			{
				TaskType.Generation => "vector-generation",
				TaskType.Validation => "vector-validation",
				_ => null
			};
			set => Type = value switch
			{
				"vector-generation" => TaskType.Generation,
				"vector-validation" => TaskType.Validation,
			};
		}

		public long VectorSetID { get; set; }
		public bool IsSample { get; set; }
		public bool ShowExpected { get; set; }
		public TaskStatus Status { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
