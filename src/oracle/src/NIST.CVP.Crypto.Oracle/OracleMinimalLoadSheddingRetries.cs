using System;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Interfaces;

namespace NIST.CVP.Crypto.Oracle
{
    public class OracleMinimalLoadSheddingRetries : Oracle
    {
        protected override int LoadSheddingRetries => 5;

        public OracleMinimalLoadSheddingRetries(
            IDbConnectionStringFactory dbConnectionStringFactory, 
            IOptions<EnvironmentConfig> environmentConfig, 
            IOptions<OrleansConfig> orleansConfig
        ) 
            : base(dbConnectionStringFactory, environmentConfig, orleansConfig)
        {
        }
    }
}
