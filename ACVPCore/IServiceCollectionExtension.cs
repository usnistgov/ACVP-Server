using ACVPCore.Providers;
using ACVPCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ACVPCore
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectACVPCore(this IServiceCollection services)
		{
			services.AddSingleton<ITestSessionService, TestSessionService>();
			services.AddSingleton<ITestSessionProvider, TestSessionProvider>();
			services.AddSingleton<IVectorSetService, VectorSetService>();
			services.AddSingleton<IVectorSetProvider, VectorSetProvider>();
			services.AddSingleton<ITaskQueueService, TaskQueueService>();
			services.AddSingleton<ITaskQueueProvider, TaskQueueProvider>();
			services.AddSingleton<IVectorSetExpectedResultsProvider, VectorSetExpectedResultsProvider>();
			services.AddSingleton<IDependencyService, DependencyService>();
			services.AddSingleton<IDependencyProvider, DependencyProvider>();
			services.AddSingleton<IOEService, OEService>();
			services.AddSingleton<IOEProvider, OEProvider>();
			services.AddSingleton<IOrganizationService, OrganizationService>();
			services.AddSingleton<IOrganizationProvider, OrganizationProvider>();
			services.AddSingleton<IAddressService, AddressService>();
			services.AddSingleton<IAddressProvider, AddressProvider>();
			services.AddSingleton<IPersonService, PersonService>();
			services.AddSingleton<IPersonProvider, PersonProvider>();
			services.AddSingleton<IImplementationService, ImplementationService>();
			services.AddSingleton<IImplementationProvider, ImplementationProvider>();
			services.AddSingleton<IValidationProvider, ValidationProvider>();
			services.AddSingleton<IValidationService, ValidationService>();
			services.AddSingleton<IScenarioAlgorithmProvider, ScenarioAlgorithmProvider>();
			services.AddSingleton<IScenarioOEProvider, ScenarioOEProvider>();
			services.AddSingleton<IScenarioProvider, ScenarioProvider>();
			services.AddSingleton<IAlgorithmService, AlgorithmService>();
			services.AddSingleton<IAlgorithmProvider, AlgorithmProvider>();
			services.AddSingleton<ICapabilityService, CapabilityService>();
			services.AddSingleton<ICapabilityProvider, CapabilityProvider>();
			services.AddSingleton<IPropertyService, PropertyService>();
			services.AddSingleton<IPropertyProvider, PropertyProvider>();
			return services;
		}
	}
}
