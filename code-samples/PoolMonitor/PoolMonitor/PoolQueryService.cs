using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PoolMonitor
{
    public class PoolQueryService : IHostedService, IDisposable
    {
        private Timer _timer = new Timer();

        private readonly int _intervalSeconds;
        private readonly string _poolUrl;
        private readonly string _outputFilePath;
        private readonly List<ParameterHolder> _poolList;
        private readonly HttpOperator _operator;
        private readonly StreamWriter _writer;

        private readonly List<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public PoolQueryService()
        {
            _poolUrl = Program.PoolUrl;
            _outputFilePath = Program.OutputFilePath;
            _intervalSeconds = Program.IntervalSeconds;
            _operator = new HttpOperator(Program.ErrorFilePath);

            _writer = new StreamWriter(_outputFilePath);

            // Hit pool information end point and store pool properties
            var poolNamesJson = _operator.Get(_poolUrl + "config").Result;
            var poolNames = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                poolNamesJson,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            ).ToList();

            foreach (var pool in poolNames)
            {
                var poolName = pool.FilePath;

                _writer.Write($"{poolName},");
            }

            _writer.WriteLine();
            _writer.Flush();

            // Get handlers for pools to query counts
            var poolList = _operator.Get(_poolUrl).Result;
            _poolList = JsonConvert.DeserializeObject<ParameterHolder[]>
            (
                poolList,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            ).ToList();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            foreach (var pool in _poolList)
            {
                var poolJson = JsonConvert.SerializeObject
                (
                    pool,
                    new JsonSerializerSettings
                    {
                        Converters = _jsonConverters
                    }
                );

                var poolCountJson = _operator.Post(_poolUrl + "status", poolJson).Result;
                var poolCount = JsonConvert.DeserializeObject<PoolInformation>(poolCountJson);

                _writer.Write($"{poolCount.FillLevel},");
            }

            _writer.WriteLine();
            _writer.Flush();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer
            {
                Interval = _intervalSeconds * 1000
            };

            _timer.Elapsed += OnTimer;
            _timer.Enabled = true;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Enabled = false;

            _writer.Flush();
            _writer.Close();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
