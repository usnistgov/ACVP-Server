using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Newtonsoft.Json;
using NLog;

namespace NIST.CVP.PoolAPI
{
    public class PoolWebHostService : WebHostService
    {
        private readonly ILogger _logger;

        public PoolWebHostService(IWebHost host) : base(host)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected override void OnStarting(string[] args)
        {
            _logger.Info("PoolWebAPI service started...");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _logger.Info("PoolWebAPI service running...");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _logger.Info("PoolWebAPI service stopped...");

            // Save Pool Data
            _logger.Info("Saving pool data...");

            var saveSuccess = Program.PoolManager.SavePools();
            if (saveSuccess)
            {
                _logger.Info("Pools saved successfully.");
            }
            else
            {
                _logger.Error("Pools failed to save!");
            }

            // Save OrleansPoolLog
            File.WriteAllText(
                Program.OrleansPoolLogLocation,
                JsonConvert.SerializeObject(Program.PoolOrleansJobLog)
            );

            base.OnStopping();
        }
    }
}
