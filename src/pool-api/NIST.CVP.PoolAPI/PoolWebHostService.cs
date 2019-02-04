using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using NIST.CVP.Pools;
using NLog;
using Microsoft.Extensions.DependencyInjection;

namespace NIST.CVP.PoolAPI
{
    public class PoolWebHostService : WebHostService
    {
        private readonly ILogger _logger;
        private readonly PoolManager _poolManager;

        public PoolWebHostService(IWebHost host) : base(host)
        {
            _poolManager = host.Services.GetService<PoolManager>();
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

            var saveSuccess = _poolManager.SavePools();
            if (saveSuccess)
            {
                _logger.Info("Pools saved successfully.");
            }
            else
            {
                _logger.Error("Pools failed to save!");
            }
            
            base.OnStopping();
        }
    }
}
