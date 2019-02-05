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

        private int _intervalSeconds;
        private string _poolUrl;
        private string _outputFilePath;
        private List<ParameterHolder> _poolList;
        private HttpOperator _operator;
        private StreamWriter _writer;

        private readonly List<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        private void OnTimer(object sender, ElapsedEventArgs args)
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _poolUrl = Program.PoolUrl;
            _outputFilePath = Program.OutputFilePath;
            _intervalSeconds = Program.IntervalSeconds;
            _operator = new HttpOperator(Program.ErrorFilePath);

            bool fileExists = File.Exists(_outputFilePath);

            _writer = new StreamWriter(_outputFilePath, true);

            if (!fileExists)
            {
                // Hit pool information end point and store pool properties
                var poolNames = JsonConvert.DeserializeObject<PoolProperties[]>
                (
                    await _operator.Get(_poolUrl + "config"),
                    new JsonSerializerSettings
                    {
                        Converters = _jsonConverters
                    }
                ).ToList();

                foreach (var pool in poolNames)
                {
                    _writer.Write($"{pool.PoolName},");
                }

                _writer.WriteLine();
                _writer.Flush();
            }

            // Get handlers for pools to query counts
            _poolList = JsonConvert.DeserializeObject<ParameterHolder[]>
            (
                await _operator.Get(_poolUrl),
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            ).ToList();

            _timer = new Timer
            {
                Interval = _intervalSeconds * 1000
            };

            _timer.Elapsed += OnTimer;
            _timer.Enabled = true;
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
