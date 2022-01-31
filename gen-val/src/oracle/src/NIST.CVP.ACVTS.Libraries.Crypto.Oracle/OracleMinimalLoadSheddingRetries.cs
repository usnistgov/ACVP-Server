using System;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public class OracleMinimalLoadSheddingRetries : Oracle
    {
        protected override int LoadSheddingRetries => 5;

        public OracleMinimalLoadSheddingRetries(
            IClusterClientFactory clusterClientFactory,
            IOptions<OrleansConfig> orleansConfig,
            IRandom800_90 random
        )
            : base(clusterClientFactory, orleansConfig, random)
        {
        }
    }
}
