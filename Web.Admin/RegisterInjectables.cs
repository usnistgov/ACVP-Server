using ACVPCore.Providers;
using ACVPCore.Services;
using ACVPWorkflow.Providers;
using ACVPWorkflow.Services;
using CVP.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Admin
{
    public static class RegisterInjectables
    {
        /// <summary>
        /// Registers all required services for the ACVP admin app.
        /// </summary>
        /// <param name="item">The service collection to manipulate.</param>
        public static void RegisterAcvpAdminServices(this IServiceCollection item)
        {
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            
            item.AddSingleton<IVectorSetExpectedResultsProvider, VectorSetExpectedResultsProvider>();
            item.AddSingleton<IVectorSetProvider, VectorSetProvider>();
            item.AddSingleton<IVectorSetService, VectorSetService>();
            
            item.AddSingleton<ITestSessionProvider, TestSessionProvider>();
            item.AddSingleton<ITestSessionService, TestSessionService>();

            item.AddSingleton<IDependencyProvider, DependencyProvider>();
            item.AddSingleton<IDependencyService, DependencyService>();

            item.AddSingleton<IOEProvider, OEProvider>();
            item.AddSingleton<IOEService, OEService>();

            item.AddSingleton<IAcvpUserProvider, AcvpUserProvider>();
            item.AddSingleton<IAcvpUserService, AcvpUserService>();
            
            item.AddSingleton<IWorkflowContactProvider, WorkflowContactProvider>();
            item.AddSingleton<IRequestProvider, RequestProvider>();
            item.AddSingleton<IWorkflowProvider, WorkflowProvider>();
            item.AddSingleton<IWorkflowService, WorkflowService>();
        }
    }
}