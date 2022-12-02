using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    /// <summary>
    /// Visitor interface for generating <see cref="IKdfParameter"/>s for the varying KAS kdf types.
    /// </summary>
    public interface IKdfParameterVisitor
    {
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="OneStepConfiguration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(OneStepConfiguration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="OneStepNoCounterConfiguration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(OneStepNoCounterConfiguration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="TwoStepConfiguration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(TwoStepConfiguration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="HkdfConfiguration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(HkdfConfiguration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="IkeV1Configuration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(IkeV1Configuration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="IkeV2Configuration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(IkeV2Configuration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="Tls10_11Configuration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(Tls10_11Configuration kdfConfiguration);
        /// <summary>
        /// Create an <see cref="IKdfParameter"/> based on a <see cref="Tls12Configuration"/>
        /// </summary>
        /// <param name="kdfConfiguration">The <see cref="IKdfConfiguration"/> to create a <see cref="IKdfParameter"/> for.</param>
        /// <returns></returns>
        IKdfParameter CreateParameter(Tls12Configuration kdfConfiguration);
    }
}
