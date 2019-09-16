using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Visitor interface for generating <see cref="IKdfParameter"/>s for the varying KAS kdf types.
    /// </summary>
    public interface IKdfParameterVisitor
    {
        /// <summary>
        /// Create an IKdfParameter based on a <see cref="OneStepConfiguration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(OneStepConfiguration kdfConfiguration);
    }
}