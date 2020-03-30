using CVP.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using Web.Public.Models;
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
        public static void RegisterAcvpPublicServices(this IServiceCollection item)
        {
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            
            item.AddTransient<ITotpService, TotpService>();
            item.AddTransient<ITotpProvider, TotpProvider>();
            item.AddTransient<IJwtService, JwtService>();

            item.AddTransient<IJsonWriterService, JsonWriterService>();
            
            item.AddTransient<IAlgorithmProvider, AlgorithmProvider>();
            item.AddTransient<IAlgorithmService, AlgorithmService>();

            item.AddTransient<IOrganizationProvider, OrganizationProvider>();
            item.AddTransient<IOrganizationService, OrganizationService>();
            item.AddTransient<IJsonReaderService<Organization>, JsonReaderService<Organization>>();

            item.AddTransient<IAddressProvider, AddressProvider>();
            item.AddTransient<IAddressService, AddressService>();
        }
    }
}