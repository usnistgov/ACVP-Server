using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Interfaces;

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
            var serviceProvider = EntryPointConfigHelper.Bootstrap(AppDomain.CurrentDomain.BaseDirectory);
            _dbConnectionStringFactory = serviceProvider.GetService<IDbConnectionStringFactory>();
            _environmentConfig = serviceProvider.GetService<IOptions<EnvironmentConfig>>();
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>();
        }

        public OracleBuilder WithDbConnectionStringFactory(IDbConnectionStringFactory value)
        {
            _dbConnectionStringFactory = value;
            return this;
        }

        public OracleBuilder WithEnvironmentConfig(IOptions<EnvironmentConfig> value)
        {
            _environmentConfig = value;
            return this;
        }

        public OracleBuilder WithOrleansConfig(IOptions<OrleansConfig> value)
        {
            _orleansConfig = value;
            return this;
        }

        public IOracle Build()
        {
            return new Oracle(
                _dbConnectionStringFactory,
                _environmentConfig,
                _orleansConfig
            );
        }
    }
}