using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Crypto.Oracle.Builders
{
    /// <summary>
    /// Builder for Oracle - at instantiation <see cref="Build"/> should return valid Oracle.
    /// Provides methods for injecting own configuration
    /// </summary>
    public class OracleBuilder
    {
        private IOptions<EnvironmentConfig> _environmentConfig;
        private IOptions<AlgorithmConfig> _algorithmConfig;
        private IOptions<PoolConfig> _poolConfig;
        private IOptions<OrleansConfig> _orleansConfig;

        public OracleBuilder()
        {
            var serviceProvider = EntryPointConfigHelper.Bootstrap(AppDomain.CurrentDomain.BaseDirectory);
            _environmentConfig = serviceProvider.GetService<IOptions<EnvironmentConfig>>();
            _algorithmConfig = serviceProvider.GetService<IOptions<AlgorithmConfig>>();
            _poolConfig = serviceProvider.GetService<IOptions<PoolConfig>>();
            _orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>();
        }

        public OracleBuilder WithEnvironmentConfig(IOptions<EnvironmentConfig> value)
        {
            _environmentConfig = value;
            return this;
        }

        public OracleBuilder WithAlgorithmConfig(IOptions<AlgorithmConfig> value)
        {
            _algorithmConfig = value;
            return this;
        }

        public OracleBuilder WithPoolConfig(IOptions<PoolConfig> value)
        {
            _poolConfig = value;
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
                _environmentConfig,
                _poolConfig,
                _orleansConfig
            );
        }
    }
}