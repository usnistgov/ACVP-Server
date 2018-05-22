using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.MonteCarlo
{
    /// <summary>
    /// Factory for providing a monte carlo tester related to pipeline/interleaved modes with TDES
    /// </summary>
    public interface IMonteCarloFactoryTdesPartitions
    {
        /// <summary>
        /// Get a monte carlo tester given the mode.
        /// </summary>
        /// <param name="mode">The mode</param>
        /// <returns></returns>
        IMonteCarloTester<MCTResult<TDES.AlgoArrayResponseWithIvs>, TDES.AlgoArrayResponseWithIvs> GetInstance(
            BlockCipherModesOfOperation mode
        );
    }
}