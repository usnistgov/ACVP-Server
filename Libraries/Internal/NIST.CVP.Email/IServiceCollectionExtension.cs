using Microsoft.Extensions.DependencyInjection;

namespace NIST.CVP.Email
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
