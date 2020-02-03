using ACVPCore;
using ACVPCore.Providers;
using ACVPCore.Services;
using ACVPWorkflow;
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
            item.InjectACVPCore();
            item.InjectACVPWorkflow();
        
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
        }
    }
}