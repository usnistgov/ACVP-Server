using System;

namespace NIST.CVP.Libraries.Internal.TaskQueue
{
	public class TaskQueueItem
	{
		public long ID { get; set; }
		public TaskType Type { get; set; }
		public long VectorSetID { get; set; }
		public bool IsSample { get; set; }
		public bool ShowExpected { get; set; }
		public TaskStatus Status { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
