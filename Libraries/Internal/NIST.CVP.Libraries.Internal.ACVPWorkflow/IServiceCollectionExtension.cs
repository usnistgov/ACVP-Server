using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Services;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
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
			services.AddSingleton<IWorkflowItemPayloadValidatorFactory, WorkflowItemPayloadValidatorFactory>();
			return services;
		}
	}
}
