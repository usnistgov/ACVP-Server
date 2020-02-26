using ACVPCore;
using CVP.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using Web.Public.Providers;

namespace Web.Public
{
    public static class RegisterInjectables
    {
        /// <summary>
        /// Registers all required services for the ACVP public app.
        /// </summary>
        /// <param name="item">The service collection to manipulate.</param>
        public static void RegisterAcvpAdminServices(this IServiceCollection item)
        {
            item.InjectACVPCore();
            //item.InjectACVPWorkflow();

            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            item.AddTransient<ITotpProvider, TotpProvider>();
        }
    }
}