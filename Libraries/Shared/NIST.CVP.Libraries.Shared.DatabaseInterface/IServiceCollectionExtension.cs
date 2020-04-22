using Microsoft.Extensions.DependencyInjection;

namespace NIST.CVP.Libraries.Shared.DatabaseInterface
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectDatabaseInterface(this IServiceCollection services)
		{
			services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
			return services;
		}
	}
}
