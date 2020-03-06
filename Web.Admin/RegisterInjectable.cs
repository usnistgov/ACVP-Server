using ACVPCore;
using ACVPWorkflow;
using CVP.DatabaseInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Admin.Auth.Models;

namespace Web.Admin
{
    public static class RegisterInjectables
    {
        /// <summary>
        /// Registers all required services for the ACVP admin app.
        /// </summary>
        /// <param name="item">The service collection to manipulate.</param>
        /// <param name="configuration">The builder configuration.</param>
        public static void RegisterAcvpAdminServices(this IServiceCollection item, IConfiguration configuration)
        {
            item.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            item.Configure<SsoConfig>(configuration.GetSection(nameof(SsoConfig)));
            
            item.InjectACVPCore();
            item.InjectACVPWorkflow();
        }
    }
}