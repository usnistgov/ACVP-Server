using Microsoft.Extensions.DependencyInjection;

namespace CVP.DatabaseInterface
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
