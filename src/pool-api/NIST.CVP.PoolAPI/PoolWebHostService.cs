using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
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
            base.OnStopping();
        }
    }
}
