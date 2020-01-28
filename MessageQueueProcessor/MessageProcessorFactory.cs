using System.Collections.Generic;
using ACVPCore.Services;
using ACVPWorkflow;
using ACVPWorkflow.Services;
using MessageQueueProcessor.MessageProcessors;

namespace MessageQueueProcessor
{
	public class MessageProcessorFactory : IMessageProcessorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowService _workflowService;
		private readonly IOEService _oeService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IImplementationService _implementationService;
		private readonly Dictionary<APIAction, bool> _autoApproveConfiguration;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly IValidationService _validationService;

		public MessageProcessorFactory(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService, IDependencyService dependencyService, IOEService oeService, IOrganizationService organizationService, IPersonService personService, IImplementationService implementationService, IAutoApproveProvider autoApproveProvider, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, IValidationService validationService)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_dependencyService = dependencyService;
			_oeService = oeService;
			_organizationService = organizationService;
			_personService = personService;
			_implementationService = implementationService;
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_validationService = validationService;
			_autoApproveConfiguration = autoApproveProvider.GetAutoApproveConfiguration();
		}

		public IMessageProcessor GetMessageProcessor(APIAction action)
		{
			//Get whether or not this action is auto approved, since most constructors need it. Will default to false if not found (like for the actions that don't have approvals)
			bool autoApprove = _autoApproveConfiguration.GetValueOrDefault(action);

			return action switch
			{
				APIAction.CreateDependency => new CreateDependencyProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.UpdateDependency => new UpdateDependencyProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.DeleteDependency => new DeleteDependencyProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.CreateImplementation => new CreateImplementationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.UpdateImplementation => new UpdateImplementationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.DeleteImplementation => new DeleteImplementationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.CreateOE => new CreateOEProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.UpdateOE => new UpdateOEProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.DeleteOE => new DeleteOEProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.CreatePerson => new CreatePersonProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.UpdatePerson => new UpdatePersonProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.DeletePerson => new DeletePersonProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.CreateVendor => new CreateOrganizationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.UpdateVendor => new UpdateOrganizationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.DeleteVendor => new DeleteOrganizationProcessor(_workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.RegisterTestSession => new RegisterTestSessionProcessor(_testSessionService, _vectorSetService, _taskQueueService),
				APIAction.CancelTestSession => new CancelTestSessionProcessor(_testSessionService),
				APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_testSessionService, _validationService, _workflowService, _workflowItemProcessorFactory, autoApprove),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsProcessor(_vectorSetService, _taskQueueService),
				APIAction.CancelVectorSet => new CancelVectorSetProcessor(_vectorSetService),
				_ => null
			};
		}
	}
}
