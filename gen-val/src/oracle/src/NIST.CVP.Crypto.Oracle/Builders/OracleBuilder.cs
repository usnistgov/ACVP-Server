using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Crypto.Oracle.Builders
{
    /// <summary>
    /// Builder for Oracle - at instantiation <see cref="Build"/> should return valid Oracle.
    /// Provides methods for injecting own configuration
    /// </summary>
    public class OracleBuilder
    {
        private IDbConnectionStringFactory _dbConnectionStringFactory;
        private IOptions<EnvironmentConfig> _environmentConfig;
        private IOptions<OrleansConfig> _orleansConfig;

        public OracleBuilder()
        {
            var serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
            _dbConnectionStringFactory = serviceProvider.GetService<IDbConnectionStringFactory>();
            _environmentConfig = serviceProvider.GetService<IOptions<EnvironmentConfig>>();
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>();
        }

        public async Task<IOracle> Build()
        {
            var oracle = new Oracle(
                new ClusterClientFactory(_dbConnectionStringFactory, _environmentConfig, _orleansConfig), 
                _orleansConfig
            );

            await oracle.InitializeClusterClient();

            return oracle;
        }
    }
}