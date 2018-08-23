using Microsoft.AspNetCore.Hosting;
using System.ServiceProcess;

namespace NIST.CVP.PoolAPI
{
    public static class WebHostServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new PoolWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
