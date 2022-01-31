using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Hosting;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public class ConfigBasedOrleansClientClustering : IOrleansClientClustering
    {
        private readonly string _orleansConnectionString;
        private readonly IOptions<EnvironmentConfig> _environmentConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;

        public ConfigBasedOrleansClientClustering(IDbConnectionStringFactory dbConnectionStringFactory, IOptions<EnvironmentConfig> environmentConfig, IOptions<OrleansConfig> orleansConfig)
        {
            _orleansConnectionString = dbConnectionStringFactory
                .GetConnectionString(Constants.OrleansConnectionString);
            _environmentConfig = environmentConfig;
            _orleansConfig = orleansConfig;
        }

        public void ConfigureClustering(IClientBuilder builder)
        {
            switch (_environmentConfig.Value.Name)
            {
                case Environments.Local:
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Tc:
                    List<IPEndPoint> endpoints = new List<IPEndPoint>();
                    foreach (var endpoint in _orleansConfig.Value.OrleansNodeConfig.Select(s => s.HostName))
                    {
                        endpoints.Add(new IPEndPoint(
                            IPAddress.Parse(endpoint), _orleansConfig.Value.OrleansGatewayPort
                        ));
                    }
                    builder.UseStaticClustering(endpoints.ToArray());
                    break;
                default:
                    builder.UseAdoNetClustering(options =>
                    {
                        options.Invariant = "System.Data.SqlClient";
                        options.ConnectionString = _orleansConnectionString;
                    });
                    break;
            }
        }
    }
}
