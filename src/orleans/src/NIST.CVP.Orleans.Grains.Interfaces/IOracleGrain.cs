using NIST.CVP.Common.Oracle;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleGrain : IOracle, IGrainWithGuidKey
    {
        
    }
}