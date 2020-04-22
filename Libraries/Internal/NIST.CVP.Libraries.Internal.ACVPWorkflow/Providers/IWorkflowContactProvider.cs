using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public interface IWorkflowContactProvider
	{
		WorkflowContact GetContactForACVPUser(long acvpUserID);
	}
}