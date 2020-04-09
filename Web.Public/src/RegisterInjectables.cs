using CVP.DatabaseInterface;
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
        public static void RegisterAcvpPublicServices(this IServiceCollection item)
        {
            item.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
            
            item.AddTransient<ITotpService, TotpService>();
            item.AddTransient<ITotpProvider, TotpProvider>();
            item.AddTransient<IJwtService, JwtService>();

            item.AddTransient<IJsonWriterService, JsonWriterService>();
            item.AddTransient<IJsonReaderService, JsonReaderService>();

            item.AddTransient<IUserProvider, UserProvider>();
            item.AddTransient<IMessageProvider, MessageProvider>();
            item.AddTransient<IMessageService, MessageService>();
            
            item.AddTransient<IAlgorithmProvider, AlgorithmProvider>();
            item.AddTransient<IAlgorithmService, AlgorithmService>();

            item.AddTransient<IDependencyProvider, DependencyProvider>();
            item.AddTransient<IDependencyService, DependencyService>();
            
            item.AddTransient<IImplementationProvider, ImplementationProvider>();
            item.AddTransient<IImplementationService, ImplementationService>();
            
            item.AddTransient<IOEProvider, OEProvider>();
            item.AddTransient<IOEService, OEService>();
            
            item.AddTransient<IPersonProvider, PersonProvider>();
            item.AddTransient<IPersonService, PersonService>();
            
            item.AddTransient<IOrganizationProvider, OrganizationProvider>();
            item.AddTransient<IOrganizationService, OrganizationService>();
            
            item.AddTransient<IAddressProvider, AddressProvider>();
            item.AddTransient<IAddressService, AddressService>();

            item.AddTransient<IRequestProvider, RequestProvider>();
            item.AddTransient<IRequestService, RequestService>();

            item.AddTransient<ITestSessionProvider, TestSessionProvider>();
            item.AddTransient<ITestSessionService, TestSessionService>();

            item.AddTransient<IVectorSetProvider, VectorSetProvider>();
            //item.AddTransient<IVectorSetService, VectorSetService>();
        }
    }
}