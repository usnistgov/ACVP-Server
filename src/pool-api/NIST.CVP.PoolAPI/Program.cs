using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("-c"));
            var builder = CreateWebHostBuilder(args.Where(arg => arg != "-c").ToArray());

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                builder.UseContentRoot(pathToContentRoot);
            }

            var host = builder.Build();

            if (isService)
            {
                host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
