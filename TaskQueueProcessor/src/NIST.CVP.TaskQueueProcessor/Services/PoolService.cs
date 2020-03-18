using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.TaskQueueProcessor.Constants;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class PoolService : IPoolService
    {
        private readonly IOptions<PoolConfig> _config;

        public PoolService(IOptions<PoolConfig> config)
        {
            _config = config;
        }
        
        public async Task<object> SpawnPoolData()
        {
            var uriBuilder = new UriBuilder
            {
                Host = _config.Value.RootUrl,
                Path = PoolApiEndPoints.SPAWN,
                Port = _config.Value.Port
            };
            
            var client = new HttpClient();

            try
            {
                // TODO right now 1 pool spawn is nowhere near as much work as 1 vector set for Orleans. This could be multiplied to get more work done during idle time
                var response = await client.GetAsync(uriBuilder.Uri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to complete request to PoolApi at {_config.Value.RootUrl}:{_config.Value.Port} with error {response.StatusCode}");
                }

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}