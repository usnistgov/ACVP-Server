using Microsoft.Extensions.DependencyInjection;

namespace NIST.CVP.Libraries.Internal.Email
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectMailer(this IServiceCollection services)
		{
			services.AddSingleton<IMailer, Mailer>();
			return services;
		}
	}
}
