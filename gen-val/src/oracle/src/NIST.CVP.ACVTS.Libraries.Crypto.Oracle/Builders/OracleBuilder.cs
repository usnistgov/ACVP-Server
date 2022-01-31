using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders
{
    /// <summary>
    /// Builder for Oracle - at instantiation <see cref="Build"/> should return valid Oracle.
    /// Provides methods for injecting own configuration
    /// </summary>
    public class OracleBuilder
    {
        private readonly IOrleansClientClustering _orleansClientClustering;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        private readonly IRandom800_90 _random;

        public OracleBuilder()
        {
            var serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
            _orleansClientClustering = new LocalOrleansClientClustering();
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>();
            _random = serviceProvider.GetService<IRandom800_90>();
        }

        public async Task<IOracle> Build()
        {
            var oracle = new Oracle(
                new ClusterClientFactory(_orleansClientClustering, _orleansConfig),
                _orleansConfig,
                _random
            );

            await oracle.InitializeClusterClient();

            return oracle;
        }
    }
}
