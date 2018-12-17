using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoolMonitor
{
    public class Program
    {
        public static void Main()
        {
            var poolUrl = "http://admin.dev.nist.gov/api/pools/";

            // Create a file for data

            // Hit pool information end point and store pool properties
            var http = new HttpOperator();
            var poolInfo = http.Get(poolUrl);

            // Run every 5 minutes (300 seconds)
            while (true)
            {
                // Iterate through pools hitting end-point


                Thread.Sleep(300 * 1000);
            }
        }
    }

    internal class HttpOperator
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> Get(string url)
        {
            var getResponse = await _client.GetAsync(url);

            return "";
        }

        public async Task<string> Post(string url, string content)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync(url, httpContent);

            return "";
        }
    }
}
