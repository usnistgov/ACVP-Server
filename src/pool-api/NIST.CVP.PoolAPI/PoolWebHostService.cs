using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NIST.CVP.PoolAPI
{
    internal class PoolWebHostService : WebHostService
    {
        private readonly ILogger _logger;

        public PoolWebHostService(IWebHost host) : base(host)
        {
            _logger = host.Services.GetRequiredService<ILogger<PoolWebHostService>>();
        }

        protected override void OnStarting(string[] args)
        {
            _logger.LogInformation("PoolWebAPI service started...");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("PoolWebAPI service running...");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("PoolWebAPI service stopped...");

            // Save Pool Data
            _logger.LogInformation("Saving pool data...");

            var saveSuccess = Startup.PoolManager.SavePools();
            if (saveSuccess)
            {
                _logger.LogInformation("Pools saved successfully.");
            }
            else
            {
                _logger.LogError("Pools failed to save!");
            }

            base.OnStopping();
        }
    }
}
