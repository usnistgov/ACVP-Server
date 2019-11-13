using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep
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
        /// <returns></returns>
        IKdfOneStep GetInstance(KasKdfOneStepAuxFunction auxFunction);
    }
}