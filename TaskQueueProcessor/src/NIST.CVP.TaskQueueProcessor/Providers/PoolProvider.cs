using System;
using System.Net.Http;
using NIST.CVP.TaskQueueProcessor.Constants;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class PoolProvider : IPoolProvider
    {
        private readonly string _uri;
        private readonly int _port;

        public PoolProvider(string uri, int port)
        {
            _uri = uri;
            _port = port;
        }
        
        public void SpawnPoolData()
        {
            var uriBuilder = new UriBuilder
            {
                Host = _uri,
                Path = PoolApiEndPoints.SPAWN,
                Port = _port
            };
            
            var client = new HttpClient();
            var response = client.GetAsync(uriBuilder.Uri);

            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to complete request to PoolApi at {_uri}:{_port} with error {response.Result.StatusCode}"); 
            }
        }
    }
}