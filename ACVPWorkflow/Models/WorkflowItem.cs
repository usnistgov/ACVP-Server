using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPWorkflow
{
	public class WorkflowItem
	{
		public long WorkflowItemID { get; set; }
		public APIAction APIAction { get; set; }
		public string JSON { get; set; }
	}
}
