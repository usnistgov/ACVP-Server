using ACVPCore.Services;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;

namespace ACVPWorkflow
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

		public WorkflowItemProcessorFactory(
			ITestSessionService testSessionService, 
			IDependencyService dependencyService, 
			IOEService oeService, 
			IOrganizationService organizationService, 
			IPersonService personService, 
			IImplementationService implementationService, 
			IValidationService validationService, 
			IVectorSetService vectorSetService)
		{
			_testSessionService = testSessionService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_organizationService = organizationService;
			_personService = personService;
			_implementationService = implementationService;
			_validationService = validationService;
			_vectorSetService = vectorSetService;
		}

		public IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action) => action switch
		{
			APIAction.CreateDependency => new CreateDependencyProcessor(_dependencyService),
			APIAction.UpdateDependency => new UpdateDependencyProcessor(_dependencyService),
			APIAction.DeleteDependency => new DeleteDependencyProcessor(_dependencyService),
			APIAction.CreateImplementation => new CreateImplementationProcessor(_implementationService),
			APIAction.UpdateImplementation => new UpdateImplementationProcessor(_implementationService),
			APIAction.DeleteImplementation => new DeleteImplementationProcessor(_implementationService),
			APIAction.CreateOE => new CreateOEProcessor(_oeService, _dependencyService),
			APIAction.UpdateOE => new UpdateOEProcessor(_oeService, _dependencyService),
			APIAction.DeleteOE => new DeleteOEProcessor(_oeService),
			APIAction.CreatePerson => new CreatePersonProcessor(_personService),
			APIAction.UpdatePerson => new UpdatePersonProcessor(_personService),
			APIAction.DeletePerson => new DeletePersonProcessor(_personService),
			APIAction.CreateVendor => new CreateOrganizationProcessor(_organizationService),
			APIAction.UpdateVendor => new UpdateOrganizationProcessor(_organizationService),
			APIAction.DeleteVendor => new DeleteOrganizationProcessor(_organizationService),
			APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_validationService, _testSessionService, _vectorSetService, _dependencyService, _oeService, _implementationService),
			_ => null
		};
	}
}
