using System;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class WorkflowItemPayloadValidatorFactory : IWorkflowItemValidatorFactory
	{
		private readonly IDependencyService _dependencyService;
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;

		public WorkflowItemPayloadValidatorFactory(IDependencyService dependencyService, IAddressService addressService, IOrganizationService organizationService, IPersonService personService)
		{
			_dependencyService = dependencyService;
			_addressService = addressService;
			_organizationService = organizationService;
			_personService = personService;
		}
		
		public IWorkflowItemValidator GetWorkflowItemPayloadValidator(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => (IWorkflowItemValidator) new DependencyCreatePayloadValidator(),
				APIAction.CreateImplementation => new ImplementationCreatePayloadValidator(_addressService, _organizationService, _personService),
				APIAction.CreateOE => new OperatingEnvironmentCreatePayloadValidator(),
				APIAction.CreatePerson => new PersonCreatePayloadValidator(),
				APIAction.CreateVendor => new VendorCreatePayloadValidator(),
				APIAction.DeleteDependency => new DependencyDeletePayloadValidator(_dependencyService),
				APIAction.DeleteImplementation => new ImplementationDeletePayloadValidator(),
				APIAction.DeleteOE => new OperatingEnvironmentDeletePayloadValidator(),
				APIAction.DeletePerson => new PersonDeletePayloadValidator(),
				APIAction.DeleteVendor => new VendorDeletePayloadValidator(),
				APIAction.UpdateDependency => new DependencyUpdatePayloadValidator(_dependencyService),
				APIAction.UpdateImplementation => new ImplementationUpdatePayloadValidator(),
				APIAction.UpdateOE => new OperatingEnvironmentUpdatePayloadValidator(),
				APIAction.UpdatePerson => new PersonUpdatePayloadValidator(),
				APIAction.UpdateVendor => new VendorUpdatePayloadValidator(),
				APIAction.RegisterTestSession => new RegisterTestSessionPayloadValidator(),
				APIAction.CertifyTestSession => new CertifyTestSessionPayloadValidator(),
				APIAction.CancelTestSession => new CancelTestSessionPayloadValidator(),
				APIAction.CancelVectorSet => new CancelVectorSetPayloadValidator(),
				APIAction.ResubmitVectorSetResults => new ResubmitVectorSetResultsPayloadValidator(),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsPayloadValidator(),
				_ => throw new ArgumentException($"Invalid {nameof(action)}: {action}")
			};
		}
	}
}