using ACVPCore.Services;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;

namespace ACVPWorkflow
{
	public class WorkflowItemProcessorFactory : IWorkflowItemProcessorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowService _workflowService;
		private readonly IOEService _oeService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IImplementationService _implementationService;
		private readonly IValidationService _validationService;

		public WorkflowItemProcessorFactory(ITestSessionService testSessionService, IDependencyService dependencyService, IOEService oeService, IOrganizationService organizationService, IPersonService personService, IImplementationService implementationService, IWorkflowService workflowService, IValidationService validationService)
		{
			_testSessionService = testSessionService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_organizationService = organizationService;
			_personService = personService;
			_implementationService = implementationService;
			_workflowService = workflowService;
			_validationService = validationService;
		}

		public IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => new CreateDependencyProcessor(_dependencyService, _workflowService),
				APIAction.UpdateDependency => new UpdateDependencyProcessor(_dependencyService, _workflowService),
				APIAction.DeleteDependency => new DeleteDependencyProcessor(_dependencyService, _workflowService),
				APIAction.CreateImplementation => new CreateImplementationProcessor(_implementationService, _workflowService),
				APIAction.UpdateImplementation => new UpdateImplementationProcessor(_implementationService, _workflowService),
				APIAction.DeleteImplementation => new DeleteImplementationProcessor(_implementationService, _workflowService),
				APIAction.CreateOE => new CreateOEProcessor(_oeService, _workflowService),
				APIAction.UpdateOE => new UpdateOEProcessor(_oeService, _workflowService),
				APIAction.DeleteOE => new DeleteOEProcessor(_oeService, _workflowService),
				APIAction.CreatePerson => new CreatePersonProcessor(_personService, _workflowService),
				APIAction.UpdatePerson => new UpdatePersonProcessor(_personService, _workflowService),
				APIAction.DeletePerson => new DeletePersonProcessor(_personService, _workflowService),
				APIAction.CreateVendor => new CreateOrganizationProcessor(_organizationService, _workflowService),
				APIAction.UpdateVendor => new UpdateOrganizationProcessor(_organizationService, _workflowService),
				APIAction.DeleteVendor => new DeleteOrganizationProcessor(_organizationService, _workflowService),
				APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_validationService, _testSessionService, _workflowService),
				_ => null
			};
		}
	}
}
