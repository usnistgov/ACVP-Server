using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Common.Symmetric.MonteCarlo
{
    /// <summary>
    /// Monte Carlo Test abstraction
    /// </summary>
    /// <typeparam name="TMonteCarloResult">The MCT Result wrapper</typeparam>
    /// <typeparam name="TAlgoArrayResponse">The individual outer loop response objects</typeparam>
    public interface IMonteCarloTester<out TMonteCarloResult, out TAlgoArrayResponse>
        where TMonteCarloResult : IMCTResult<TAlgoArrayResponse>
        where TAlgoArrayResponse : ICryptoResult
    {
        /// <summary>
        /// Process a monte carlo test given the parameters.
        /// </summary>
        /// <param name="param">The starting parameters of the MCT test</param>
        /// <returns></returns>
        TMonteCarloResult ProcessMonteCarloTest(IModeBlockCipherParameters param);
    }
}