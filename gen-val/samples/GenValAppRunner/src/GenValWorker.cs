using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;          
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Generation;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;


    public class GenValWorker : BackgroundService
    {
        private readonly ILogger<GenValWorker> _logger;
        private readonly IConnectionMultiplexer _redis;
        private readonly IServiceProvider _services;

        public GenValWorker(ILogger<GenValWorker> logger, IConnectionMultiplexer redis, IServiceProvider services)
        {
            _logger = logger;
            _redis = redis;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          var db = _redis.GetDatabase();
           _logger.LogInformation("GenValWorker started and listening for jobs...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await db.ListLeftPopAsync("genval:requests");

            if (!result.HasValue)
            {
                await Task.Delay(100, stoppingToken); // small wait before checking again
                continue;
            }

            GenValRequestJob? job;
            try
            {
                job = JsonConvert.DeserializeObject<GenValRequestJob>(result!);
                if (job == null)
                {
                    _logger.LogWarning("Invalid job format. Skipping.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize GenValRequestJob.");
                continue;
            }

            _logger.LogInformation($"Processing Job with id = {job.JobId} and operation={job.Operation}");

            var response = new GenValResponseJob
            {
                JobId = job.JobId,
                Operation = job.Operation
            };

            try
            {
                response.Result = job.Operation switch
                {
                    "get-supported-algorithms" => JsonConvert.SerializeObject(AutofacConfig.GetSupportedAlgoModeInfos()),
                    // "generate" 
                    // "validate" 
                    _ => throw new NotSupportedException($"Unsupported operation: {job.Operation}")
                };

                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing job {job.JobId}");
                response.Success = false;
                response.Error = ex.Message;
                response.Result = string.Empty;
            }

            var responseJson = JsonConvert.SerializeObject(response);
            await db.ListRightPushAsync("veep:responses", responseJson);
            }
        }
    }
