using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.MonteCarlo
{
    /// <summary>
    /// Factory for providing a monte carlo tester
    /// </summary>
    public interface IMonteCarloFactoryAes
    {
        /// <summary>
        /// Get a monte carlo tester given the mode.
        /// </summary>
        /// <param name="mode">The mode</param>
        /// <returns></returns>
        IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> GetInstance(
            BlockCipherModesOfOperation mode
        );
    }
}