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
		private readonly IAlgorithmService _algorithmService;
		private readonly IValidationService _validationService;

		public MessageProcessorFactory(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService, IDependencyService dependencyService, IOEService oeService, IOrganizationService organizationService, IPersonService personService, IImplementationService implementationService, IAutoApproveProvider autoApproveProvider, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, IValidationService validationService, IAlgorithmService algorithmService)
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
			_algorithmService = algorithmService;
			_autoApproveConfiguration = autoApproveProvider.GetAutoApproveConfiguration();
		}

		public IMessageProcessor GetMessageProcessor(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.UpdateDependency => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.DeleteDependency => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.CreateImplementation => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.UpdateImplementation => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.DeleteImplementation => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.CreateOE => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.UpdateOE => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.DeleteOE => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.CreatePerson => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.UpdatePerson => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.DeletePerson => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.CreateVendor => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.UpdateVendor => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.DeleteVendor => new RequestProcessor(_workflowService, _workflowItemProcessorFactory, _autoApproveConfiguration),
				APIAction.RegisterTestSession => new RegisterTestSessionProcessor(_testSessionService, _vectorSetService, _taskQueueService),
				APIAction.CancelVectorSet => new CancelVectorSetProcessor(_vectorSetService),
				APIAction.CancelTestSession => new CancelTestSessionProcessor(_testSessionService),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsProcessor(_vectorSetService, _taskQueueService),
				APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_testSessionService, _validationService, _workflowService, _workflowItemProcessorFactory, _algorithmService, _autoApproveConfiguration.GetValueOrDefault(APIAction.CertifyTestSession)),
				_ => null
			};
		}
	}
}
