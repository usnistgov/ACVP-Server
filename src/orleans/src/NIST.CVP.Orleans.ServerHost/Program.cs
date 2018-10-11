using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("console"));

            if (isService)
            {
                ServiceBase.Run(new OrleansServerService());
            }
            else
            {
                var consoleHost = new OrleansSiloHost(AppDomain.CurrentDomain.BaseDirectory);
                consoleHost.StartSilo();
                
                Console.ReadKey();

                consoleHost.StopSilo();
            }
        }
    }
}
