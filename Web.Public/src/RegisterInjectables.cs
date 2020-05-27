using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Generation;
using NIST.CVP.Generation.Core;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Web.Public.Providers;
using Web.Public.Services;
using Web.Public.Services.MessagePayloadValidators;

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
            
            item.AddSingleton<ISecretKvpProvider, SecretKvpProvider>();
            
            item.AddSingleton<ITotpService, TotpService>();
            item.AddSingleton<ITotpProvider, TotpProvider>();
            item.AddSingleton<IJwtService, JwtService>();

            item.AddSingleton<IJsonWriterService, JsonWriterService>();
            item.AddSingleton<IJsonReaderService, JsonReaderService>();

            item.AddSingleton<IUserProvider, UserProvider>();
            item.AddSingleton<IMessageProvider, MessageProvider>();
            item.AddSingleton<IMessageService, MessageService>();
            
            item.AddSingleton<IAlgorithmProvider, AlgorithmProvider>();
            item.AddSingleton<IAlgorithmService, AlgorithmService>();

            item.AddSingleton<IDependencyProvider, DependencyProvider>();
            item.AddSingleton<IDependencyService, DependencyService>();
            
            item.AddSingleton<IImplementationProvider, ImplementationProvider>();
            item.AddSingleton<IImplementationService, ImplementationService>();
            
            item.AddSingleton<IOEProvider, OEProvider>();
            item.AddSingleton<IOEService, OEService>();
            
            item.AddSingleton<IPersonProvider, PersonProvider>();
            item.AddSingleton<IPersonService, PersonService>();
            
            item.AddSingleton<IOrganizationProvider, OrganizationProvider>();
            item.AddSingleton<IOrganizationService, OrganizationService>();
            
            item.AddSingleton<IAddressProvider, AddressProvider>();
            item.AddSingleton<IAddressService, AddressService>();

            item.AddSingleton<IRequestProvider, RequestProvider>();
            item.AddSingleton<IRequestService, RequestService>();

            item.AddSingleton<IGenValInvoker, GenValInvoker>();
            item.AddSingleton<IParameterValidatorService, ParameterValidatorService>();
            
            item.AddSingleton<ITestSessionProvider, TestSessionProvider>();
            item.AddSingleton<ITestSessionService, TestSessionService>();

            item.AddSingleton<IVectorSetProvider, VectorSetProvider>();
            item.AddSingleton<IVectorSetService, VectorSetService>();

            item.AddSingleton<IMessagePayloadValidatorFactory, MessagePayloadValidatorFactory>();
        }
    }
}