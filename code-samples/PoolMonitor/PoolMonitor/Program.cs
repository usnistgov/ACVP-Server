using Newtonsoft.Json;
using NIST.CVP.Pools.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using NIST.CVP.Generation.Core.JsonConverters;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;

namespace PoolMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var isService = !(Debugger.IsAttached || args.Contains("--console"));

                        
            //var builder = new HostBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddHostedService<PoolQueryService>();
            //    });

            //if (isService)
            //{
            //    await builder.RunAsServiceAsync();
            //}
            //else
            //{
            //    await builder.RunConsoleAsync();
            //}

            // Can probably make this a parameter to expand to other environments
            var poolUrl = "http://admin.dev.acvts.nist.gov:5002/api/pools/";

            var jsonConverters = new List<JsonConverter>
            {
                new BitstringConverter(),
                new DomainConverter(),
                new BigIntegerConverter(),
                new StringEnumConverter()
            };

            // Create a file for data
            var outputFile = File.CreateText("path.csv");

            // Hit pool information end point and store pool properties
            var http = new HttpOperator();
            var poolInfo = http.Get(poolUrl).Result;

            var poolList = JsonConvert.DeserializeObject<ParameterHolder[]>
            (
                poolInfo,
                new JsonSerializerSettings
                {
                    Converters = jsonConverters
                }
            );

            // Run every 5 minutes (300 seconds)
            //var timer = new System.Timers.Timer();
            //timer.Interval = 300 * 1000;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            //timer.Start();

            while (true)
            {
                // Iterate through pools hitting end-point
                foreach (var pool in poolList)
                {

                    outputFile.WriteLine();
                }

                Thread.Sleep(300 * 1000);
            }
        }
    }

    internal class HttpOperator
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> Get(string url)
        {
            try
            {
                var getResponse = await _client.GetAsync(url);


                if (getResponse.IsSuccessStatusCode)
                {
                    return await getResponse.Content.ReadAsStringAsync();
                }

                return getResponse.StatusCode.ToString();
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> Post(string url, string content)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync(url, httpContent);

            return await postResponse.Content.ReadAsStringAsync();
        }
    }
}
