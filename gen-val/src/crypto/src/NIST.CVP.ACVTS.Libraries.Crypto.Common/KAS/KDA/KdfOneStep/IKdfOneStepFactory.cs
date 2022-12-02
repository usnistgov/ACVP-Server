using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep
{
    /// <summary>
    /// Describes retrieving a <see cref="IKdfOneStep"/> instance
    /// </summary>
    public interface IKdfOneStepFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IKdfOneStep"/> based on the specified parameters.
        /// </summary>
        /// <param name="auxFunction">The hash/mac function used in the KDF.</param>
        /// <param name="useCounter">Should implementation utilize a counter?
        /// (If false, then repetitions are limit to 1, and the 32 bit counter is not used in the concatenation step.)
        /// </param>
        /// <returns></returns>
        IKdfOneStep GetInstance(KdaOneStepAuxFunction auxFunction, bool useCounter);
    }
}
