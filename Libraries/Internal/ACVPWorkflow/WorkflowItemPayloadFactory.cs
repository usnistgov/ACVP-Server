using System.Text.Json;
using ACVPWorkflow.Models;

namespace ACVPWorkflow
{
	public class WorkflowItemPayloadFactory : IWorkflowItemPayloadFactory
	{
		public IWorkflowItemPayload GetPayload(string payload, APIAction action) => action switch
		{
			APIAction.CreateDependency => JsonSerializer.Deserialize<DependencyCreatePayload>(payload),
			APIAction.UpdateDependency => JsonSerializer.Deserialize<DependencyUpdatePayload>(payload),
			APIAction.DeleteDependency => JsonSerializer.Deserialize<DeletePayload>(payload),
			APIAction.CreateImplementation => JsonSerializer.Deserialize<ImplementationCreatePayload>(payload),
			APIAction.UpdateImplementation => JsonSerializer.Deserialize<ImplementationUpdatePayload>(payload),
			APIAction.DeleteImplementation => JsonSerializer.Deserialize<DeletePayload>(payload),
			APIAction.CreateOE => JsonSerializer.Deserialize<OECreatePayload>(payload),
			APIAction.UpdateOE => JsonSerializer.Deserialize<OEUpdatePayload>(payload),
			APIAction.DeleteOE => JsonSerializer.Deserialize<DeletePayload>(payload),
			APIAction.CreatePerson => JsonSerializer.Deserialize<PersonCreatePayload>(payload),
			APIAction.UpdatePerson => JsonSerializer.Deserialize<PersonUpdatePayload>(payload),
			APIAction.DeletePerson => JsonSerializer.Deserialize<DeletePayload>(payload),
			APIAction.CreateVendor => JsonSerializer.Deserialize<OrganizationCreatePayload>(payload),
			APIAction.UpdateVendor => JsonSerializer.Deserialize<OrganizationUpdatePayload>(payload),
			APIAction.DeleteVendor => JsonSerializer.Deserialize<DeletePayload>(payload),
			APIAction.CertifyTestSession => JsonSerializer.Deserialize<CertifyTestSessionPayload>(payload),
			_ => null
		};

		public string SerializePayload(IWorkflowItemPayload payload) => payload switch
		{
			CertifyTestSessionPayload p => JsonSerializer.Serialize<CertifyTestSessionPayload>(p),
			DeletePayload p => JsonSerializer.Serialize<DeletePayload>(p),
			DependencyCreatePayload p => JsonSerializer.Serialize<DependencyCreatePayload>(p),
			DependencyUpdatePayload p => JsonSerializer.Serialize<DependencyUpdatePayload>(p),
			ImplementationCreatePayload p => JsonSerializer.Serialize<ImplementationCreatePayload>(p),
			ImplementationUpdatePayload p => JsonSerializer.Serialize<ImplementationUpdatePayload>(p),
			OECreatePayload p => JsonSerializer.Serialize<OECreatePayload>(p),
			OEUpdatePayload p => JsonSerializer.Serialize<OEUpdatePayload>(p),
			OrganizationCreatePayload p => JsonSerializer.Serialize<OrganizationCreatePayload>(p),
			OrganizationUpdatePayload p => JsonSerializer.Serialize<OrganizationUpdatePayload>(p),
			PersonCreatePayload p => JsonSerializer.Serialize<PersonCreatePayload>(p),
			PersonUpdatePayload p => JsonSerializer.Serialize<PersonUpdatePayload>(p),
			_ => null
		};
	}
}
