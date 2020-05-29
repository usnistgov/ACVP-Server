using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.TaskQueueProcessor.Constants;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class PoolService : IPoolService
    {
        private readonly IOptions<PoolConfig> _config;
        private readonly HttpClient _httpClient;

        public PoolService(IOptions<PoolConfig> config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient(GetType().FullName);
            _httpClient.BaseAddress = new UriBuilder
            {
                Host = _config.Value.RootUrl,
                Path = PoolApiEndPoints.SPAWN,
                Port = _config.Value.Port
            }.Uri;
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task SpawnPoolDataAsync()
        {
            try
            {
                // TODO right now 1 pool spawn is nowhere near as much work as 1 vector set for Orleans. This could be multiplied to get more work done during idle time
                // note the 1 under string content signifies to queue a single pool value
                var response = await _httpClient.PostAsync(_httpClient.BaseAddress.AbsoluteUri, 
                    new StringContent("1", Encoding.UTF8, "application/json"));
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to complete request to PoolApi at {_config.Value.RootUrl}:{_config.Value.Port} with error {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Cannot contact PoolAPI");
            }
        }
    }
}