using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoolMonitor
{
    public class HttpOperator
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly StreamWriter _httpErrorWriter;

        public HttpOperator(string errorPath)
        {
            _httpErrorWriter = new StreamWriter(errorPath);
        }

        /// <summary>
        /// Try to send GET request, wait 5 minutes if unavailable
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> Get(string url)
        {
            while (true)
            {
                var getResponse = await _client.GetAsync(url);

                if (getResponse.IsSuccessStatusCode)
                {
                    return await getResponse.Content.ReadAsStringAsync();
                }

                _httpErrorWriter.WriteLine($"Unable to perform GET on {url}... trying again in 5 minutes");
                Thread.Sleep(300 * 1000);
            }
        }

        /// <summary>
        /// Try to send POST request, wait 5 minutes if unavailable
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> Post(string url, string content)
        {
            while (true)
            {
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                var postResponse = await _client.PostAsync(url, httpContent);

                if (postResponse.IsSuccessStatusCode)
                {
                    return await postResponse.Content.ReadAsStringAsync();
                }

                _httpErrorWriter.WriteLine($"Unable to perform POST on {url} with {content}... trying again in 5 minutes");
                Thread.Sleep(300 * 1000);
            }
        }
    }
}
