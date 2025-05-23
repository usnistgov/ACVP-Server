using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        _logger.LogInformation("GenValWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await db.ListLeftPopAsync("genval:requests");
            if (!result.HasValue)
            {
                await Task.Delay(100, stoppingToken);
                continue;
            }

            var job = JsonConvert.DeserializeObject<GenValRequestJob>(result!);

            using var scope = _services.CreateScope();
            var invoker = new GenValInvoker(scope.ServiceProvider);

            _logger.LogInformation($"Processing job for vsId: {job.VsId} with op: {job.Operation}");

            string responseJson = job.Operation switch
            {
                "generate" => JsonConvert.SerializeObject(await invoker.GenerateAsync(job.Payload)),
                "validate" => JsonConvert.SerializeObject(await invoker.ValidateAsync(job.Payload)),
                "get-supported-algorithms" => JsonConvert.SerializeObject(invoker.GetSupportedAlgorithms()),
                _ => throw new NotSupportedException($"Unsupported operation {job.Operation}")
            };

            var response = new
            {
                vsId = job.VsId,
                operation = job.Operation,
                result = responseJson
            };

            var responseStr = JsonConvert.SerializeObject(response);
            await db.ListRightPushAsync("veep:responses", responseStr);
        }
    }
}