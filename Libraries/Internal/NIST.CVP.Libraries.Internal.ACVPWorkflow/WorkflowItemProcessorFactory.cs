using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public class WorkflowItemProcessorFactory : IWorkflowItemProcessorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IDependencyService _dependencyService;
		private readonly IOEService _oeService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IImplementationService _implementationService;
		private readonly IValidationService _validationService;
		private readonly IVectorSetService _vectorSetService;
		private readonly IAddressService _addressService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public WorkflowItemProcessorFactory(
			ITestSessionService testSessionService,
			IDependencyService dependencyService,
			IOEService oeService,
			IOrganizationService organizationService,
			IPersonService personService,
			IImplementationService implementationService,
			IValidationService validationService,
			IVectorSetService vectorSetService,
			IAddressService addressService,
			IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_testSessionService = testSessionService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_organizationService = organizationService;
			_personService = personService;
			_implementationService = implementationService;
			_validationService = validationService;
			_vectorSetService = vectorSetService;
			_addressService = addressService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action) => action switch
		{
			APIAction.CreateDependency => new CreateDependencyProcessor(_dependencyService, _workflowItemPayloadValidatorFactory),
			APIAction.UpdateDependency => new UpdateDependencyProcessor(_dependencyService, _workflowItemPayloadValidatorFactory),
			APIAction.DeleteDependency => new DeleteDependencyProcessor(_dependencyService, _workflowItemPayloadValidatorFactory),
			APIAction.CreateImplementation => new CreateImplementationProcessor(_implementationService, _addressService, _workflowItemPayloadValidatorFactory),
			APIAction.UpdateImplementation => new UpdateImplementationProcessor(_implementationService, _workflowItemPayloadValidatorFactory),
			APIAction.DeleteImplementation => new DeleteImplementationProcessor(_implementationService, _workflowItemPayloadValidatorFactory),
			APIAction.CreateOE => new CreateOEProcessor(_oeService, _dependencyService, _workflowItemPayloadValidatorFactory),
			APIAction.UpdateOE => new UpdateOEProcessor(_oeService, _dependencyService, _workflowItemPayloadValidatorFactory),
			APIAction.DeleteOE => new DeleteOEProcessor(_oeService, _workflowItemPayloadValidatorFactory),
			APIAction.CreatePerson => new CreatePersonProcessor(_personService, _workflowItemPayloadValidatorFactory),
			APIAction.UpdatePerson => new UpdatePersonProcessor(_personService, _workflowItemPayloadValidatorFactory),
			APIAction.DeletePerson => new DeletePersonProcessor(_personService, _workflowItemPayloadValidatorFactory),
			APIAction.CreateVendor => new CreateOrganizationProcessor(_organizationService, _workflowItemPayloadValidatorFactory),
			APIAction.UpdateVendor => new UpdateOrganizationProcessor(_organizationService, _workflowItemPayloadValidatorFactory),
			APIAction.DeleteVendor => new DeleteOrganizationProcessor(_organizationService, _workflowItemPayloadValidatorFactory),
			APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_validationService, _testSessionService, _vectorSetService, _dependencyService, _oeService, _implementationService, _workflowItemPayloadValidatorFactory),
			_ => null
		};
	}
}
