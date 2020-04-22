using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using Web.Public.Providers;
using Web.Public.Services;

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
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            
            item.AddTransient<ITotpService, TotpService>();
            item.AddTransient<ITotpProvider, TotpProvider>();

            item.AddTransient<IJwtService, JwtService>();

            item.AddTransient<IAlgorithmProvider, AlgorithmProvider>();
        }
    }
}