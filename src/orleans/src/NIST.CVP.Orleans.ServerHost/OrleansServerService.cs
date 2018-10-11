using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.ServerHost.ExtensionMethods;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace NIST.CVP.Orleans.ServerHost
{
    public class OrleansServerService : ServiceBase
    {
        private readonly OrleansSiloHost _host;

        public OrleansServerService()
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            _host = new OrleansSiloHost(pathToContentRoot);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            _host.StartSilo();
        }

        protected override void OnStop()
        {
            _host.StopSilo();
        }
    }
}
