using ACVPCore.Providers;
using ACVPCore.Services;
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
            
            item.AddSingleton<IAcvpUserProvider, AcvpUserProvider>();
            item.AddSingleton<IAcvpUserService, AcvpUserService>();
        }
    }
}