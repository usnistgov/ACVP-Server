using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NIST.CVP.Orleans.ServerHost.ExtensionMethods
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, ServiceBaseLifetime>());
        }

        public static Task RunAsServiceAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseServiceBaseLifetime().Build().RunAsync(cancellationToken);
        }
    }
}
