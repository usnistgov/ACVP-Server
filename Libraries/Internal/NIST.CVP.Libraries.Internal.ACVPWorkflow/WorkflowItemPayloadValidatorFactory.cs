using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public class WorkflowItemPayloadValidatorFactory : IWorkflowItemPayloadValidatorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IDependencyService _dependencyService;
		private readonly IOEService _oeService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IImplementationService _implementationService;
		private readonly IAddressService _addressService;

		public WorkflowItemPayloadValidatorFactory(
			ITestSessionService testSessionService, 
			IDependencyService dependencyService, 
			IOEService oeService, 
			IOrganizationService organizationService, 
			IPersonService personService, 
			IImplementationService implementationService,
			IAddressService addressService)
		{
			_testSessionService = testSessionService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_organizationService = organizationService;
			_personService = personService;
			_implementationService = implementationService;
			_addressService = addressService;
		}

		public IWorkflowItemPayloadValidator GetWorkflowItemPayloadValidator(APIAction action) => action switch
		{
			APIAction.CreateDependency => new CreateDependencyPayloadValidator(),
			APIAction.UpdateDependency => new UpdateDependencyPayloadValidator(_dependencyService),
			APIAction.DeleteDependency => new DeleteDependencyPayloadValidator(_dependencyService),
			APIAction.CreateImplementation => new CreateImplementationPayloadValidator(_organizationService, _personService, _addressService),
			APIAction.UpdateImplementation => new UpdateImplementationPayloadValidator(_implementationService, _organizationService, _personService, _addressService),
			APIAction.DeleteImplementation => new DeleteImplementationPayloadValidator(_implementationService),
			APIAction.CreateOE => new CreateOEPayloadValidator(_dependencyService, this),
			APIAction.UpdateOE => new UpdateOEPayloadValidator(_oeService, _dependencyService, this),
			APIAction.DeleteOE => new DeleteOEPayloadValidator(_oeService),
			APIAction.CreatePerson => new CreatePersonPayloadValidator(_organizationService),
			APIAction.UpdatePerson => new UpdatePersonPayloadValidator(_personService, _organizationService),
			APIAction.DeletePerson => new DeletePersonPayloadValidator(_personService),
			APIAction.CreateVendor => new CreateOrganizationPayloadValidator(_organizationService),
			APIAction.UpdateVendor => new UpdateOrganizationPayloadValidator(_organizationService),
			APIAction.DeleteVendor => new DeleteOrganizationPayloadValidator(_organizationService),
			APIAction.CertifyTestSession => new CertifyTestSessionPayloadValidator(_implementationService, _oeService, _testSessionService, this),
			_ => null
		};
	}
}
