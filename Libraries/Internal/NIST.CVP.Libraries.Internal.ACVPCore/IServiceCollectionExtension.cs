using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;

namespace NIST.CVP.Libraries.Internal.ACVPCore
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectACVPCore(this IServiceCollection services)
		{
			services.AddSingleton<IAdminUserProvider, AdminUserProvider>();
			services.AddSingleton<IAdminUserService, AdminUserService>();
			services.AddSingleton<IAcvpUserProvider, AcvpUserProvider>();
			services.AddSingleton<IAcvpUserService, AcvpUserService>();
			services.AddSingleton<ITestSessionService, TestSessionService>();
			services.AddSingleton<ITestSessionProvider, TestSessionProvider>();
			services.AddSingleton<IVectorSetService, VectorSetService>();
			services.AddSingleton<IVectorSetProvider, VectorSetProvider>();
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
			services.AddSingleton<IValidationOEAlgorithmProvider, ValidationOEAlgorithmProvider>();
			services.AddSingleton<IScenarioOEProvider, ScenarioOEProvider>();
			services.AddSingleton<IScenarioProvider, ScenarioProvider>();
			services.AddSingleton<IAlgorithmService, AlgorithmService>();
			services.AddSingleton<IAlgorithmProvider, AlgorithmProvider>();
			services.AddSingleton<ICapabilityService, CapabilityService>();
			services.AddSingleton<ICapabilityProvider, CapabilityProvider>();
			services.AddSingleton<IPropertyService, PropertyService>();
			services.AddSingleton<IPropertyProvider, PropertyProvider>();
			services.AddSingleton<IPrerequisiteProvider, PrerequisiteProvider>();
			services.AddSingleton<IPrerequisiteService, PrerequisiteService>();
			return services;
		}
	}
}
