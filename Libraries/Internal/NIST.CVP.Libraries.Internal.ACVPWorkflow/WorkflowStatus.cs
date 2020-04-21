using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public enum WorkflowStatus
	{
		Pending = 0,
		Incomplete = 1,
		Approved = 2,
		Rejected = 3
	}
}
