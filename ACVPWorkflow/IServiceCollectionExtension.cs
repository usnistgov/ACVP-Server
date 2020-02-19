using ACVPWorkflow.Adapters;
using ACVPWorkflow.Providers;
using ACVPWorkflow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ACVPWorkflow
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectACVPWorkflow(this IServiceCollection services)
		{
			services.AddSingleton<IWorkflowService, WorkflowService>();
			services.AddSingleton<IWorkflowProvider, WorkflowProvider>();
			services.AddSingleton<IWorkflowContactProvider, WorkflowContactProvider>();
			services.AddSingleton<IRequestProvider, RequestProvider>();
			services.AddSingleton<IWorkflowItemProcessorFactory, WorkflowItemProcessorFactory>();
			services.AddSingleton<IWorkflowItemPayloadFactory, WorkflowItemPayloadFactory>();
			services.AddSingleton<IWorkflowApproveRejectAdapter, WorkflowApproveRejectAdapter>();
			services.AddSingleton<IWorkflowItemPayloadValidatorFactory, WorkflowItemPayloadValidatorFactory>();
			return services;
		}
	}
}
