using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace PoolMonitor
{
    public class PoolQueryService : IHostedService
    {
        private Timer _timer = new Timer();

        public PoolQueryService(string poolUrl, string outputFile)
        {

        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
