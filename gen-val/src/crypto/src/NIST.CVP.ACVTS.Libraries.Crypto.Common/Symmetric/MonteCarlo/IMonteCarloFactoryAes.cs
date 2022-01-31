using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo
{
    /// <summary>
    /// Factory for providing a monte carlo tester for the AES engine
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
